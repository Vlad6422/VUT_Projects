/***************************************/
/*AUTHOR :Malashchuk Vladyslav-xmalas04*/
/*IOS – projekt 2 (synchronizace) Pošta*/
/***************************************/
/*
Spuštění:
$ ./proj2 NZ NU TZ TU F
• NZ: počet zákazníků
• NU: počet úředníků
• TZ: Maximální čas v milisekundách, po který zákazník po svém vytvoření čeká, než vejde na
poštu (eventuálně odchází s nepořízenou). 0<=TZ<=10000
• TU: Maximální délka přestávky úředníka v milisekundách. 0<=TU<=100
• F: Maximální čas v milisekundách, po kterém je uzavřena pošta pro nově příchozí.
0<=F<=10000
*/
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <semaphore.h>
#include <sys/sem.h>
#include <sys/mman.h>
#include <sys/stat.h>
#include <sys/wait.h>
#include <fcntl.h>
#include <limits.h>
#include <time.h>
#include <sys/types.h>
#include <sys/ipc.h>
#include <sys/shm.h>

// Сreating shared memory pointers and semaphores

typedef struct {
    int nz;                // Zakaznik Id
    int nu;                // Urednik Id
    int tz;                // Maximální čas v milisekundách, po který zákazník po svém vytvoření čeká, než vejde napoštu (eventuálně odchází s nepořízenou).
    int tu;                // Maximální délka přestávky úředníka v milisekundách
    int f;                 // Maximální čas v milisekundách, po kterém je uzavřena pošta pro nově příchozí
    int action_counter;    // Counter pro output
    int number_service;    // Number Service
    int zakaznik;          // počet zákazníků
    int count_service;     // Count Service
    int count_1;           // Count Service 1
    int count_2;           // Count Service 2
    int count_3;           // Count Service 3
    int open;              // Posta otavrena = 1  || Zavrena = 0

    sem_t print_sem;       // for printout
    sem_t zakaznik_waiting; // wait
    sem_t test;
    sem_t test2;
    sem_t test3;
} Shared_mem;

int shm_id = 0; // global values only for shmem
Shared_mem *shm; // our shmem

void error(char *error, int err_code){
    fprintf(stderr, "%s\n", error);
    exit(err_code);
}

void create_shared_memory()
{
    
    shm_id = shmget(IPC_PRIVATE, sizeof(Shared_mem), IPC_CREAT | 0666);
    if(shm_id < 0){
        fprintf(stderr,"ERROR!\nShared memory couldn't be created.\n");
        exit(EXIT_FAILURE);
    }
    shm = (Shared_mem*)shmat(shm_id, NULL, 0);
    if(shm == (Shared_mem *) -1){
        fprintf(stderr,"ERROR!\nShared memory couldn't be created.\n");
        exit(EXIT_FAILURE);
    }
    return;
}

void delete_shared_memory () {  // destroy semaphores

    if (sem_destroy(&shm->print_sem) == -1)              error("ERROR!\nsem_destroy failed",EXIT_FAILURE);
    if (sem_destroy(&shm->zakaznik_waiting) == -1)       error("ERROR!\nsem_destroy failed",EXIT_FAILURE);
if (sem_destroy(&shm->test) == -1)       error("ERROR!\nsem_destroy failed",EXIT_FAILURE);
if (sem_destroy(&shm->test2) == -1)       error("ERROR!\nsem_destroy failed",EXIT_FAILURE);
if (sem_destroy(&shm->test3) == -1)       error("ERROR!\nsem_destroy failed",EXIT_FAILURE);
    if(shmdt(shm)==-1)
    {
        fprintf(stderr,"err_print!\nshmdt failed.\n");
        return;
    }
    shmctl(shm_id, IPC_RMID, NULL);
}

void set_value(){ //setup nedeed values
    shm->action_counter = 1;
    shm->number_service = 1;
    shm->count_service = 0;
    shm->count_1 = 0;
    shm->count_2 = 0;
    shm->count_3 = 0;
    shm->open = 1;
}

FILE* open_file() // open file
{
    FILE *f = fopen("proj2.out", "w");
    if(f == NULL)
    {
        fprintf(stderr, "ERROR!\nProblem with openning file\n");
        exit(EXIT_FAILURE);
    }
    return f;
}

void sleeping(int arg) // sleeping for arg msec. ( TZ or TU )
{
    int timeout = (rand() % (arg + 1)) * 1000; 
    usleep(timeout);
}

void sleepingF(int arg) // sleeping for arg msec. ( F )
{
    double rand_num = (double) rand() / RAND_MAX;
    int delay = (int) ((arg/2) + rand_num * (arg/2)*1000);
    usleep(delay);
}

void sem_initialize(){  // initialize semaphores 
    if (sem_init(&shm->print_sem,1,1) == -1)                  error("ERROR!\nsem_init failed",EXIT_FAILURE);
    if (sem_init(&shm->zakaznik_waiting,1,0) == -1)           error("ERROR!\nsem_init failed",EXIT_FAILURE); 
    if (sem_init(&shm->test,1,1) == -1)           error("ERROR!\nsem_init failed",EXIT_FAILURE);
    if (sem_init(&shm->test2,1,1) == -1)           error("ERROR!\nsem_init failed",EXIT_FAILURE);
    if (sem_init(&shm->test3,1,1) == -1)           error("ERROR!\nsem_init failed",EXIT_FAILURE);
    return;
}

void print(FILE *f, char* s, int n1, int n2) { // Upgraded print
    sem_wait(&shm->print_sem);
    fprintf(f,"%d: ", shm->action_counter);
    fprintf(f, s, n1, n2);
    shm->action_counter++;
    sem_post(&shm->print_sem);	
}

int inputValidation(char **argv)
{
//Pokud některý ze vstupů nebude odpovídat očekávanému formátu nebo bude mimo povolený
//rozsah, program vytiskne chybové hlášení na standardní chybový výstup, uvolní všechny dosud
//alokované zdroje a ukončí se s kódem (exit code) 1.

    int argv1 = atoi(argv[1]);
    int argv2 = atoi(argv[2]);
    int argv3 = atoi(argv[3]);
    int argv4 = atoi(argv[4]);
    int argv5 = atoi(argv[5]);
    
    if (
    ((argv1 == 0 && *argv[1] != '0') ||
     (argv2 == 0 && *argv[2] != '0') ||
     (argv3 == 0 && *argv[3] != '0') ||
     (argv4 == 0 && *argv[4] != '0') ||
     (argv5 == 0 && *argv[5] != '0'))) {
        fprintf(stderr, "Spatny format dat \n");
    return 1;
}
    // checks number of NZ and NU on input
    if (argv1 <= 0 || argv2 <= 0){
        fprintf(stderr, "Spatny format dat \n");
        return 1;
    }
    // checks if time is in correct range
    else if (argv3 < 0 || argv3 > 10000 || argv4 < 0 || argv4 > 100 || argv5 < 0 || argv5 > 10000){
        fprintf(stderr, "Chyba vstupu: spatny rozsah casu \n");
        return 1;
    }
    else{
        return 0;
    }
}
void Zakaznik(FILE *f,int z_id){
    srand(getpid() * time(NULL));
    print(f, "Z %d: started\n", z_id, 0);          // Po spuštění vypíše: A: Z idZ: started
    sleeping(shm->tz);                             // čeká pomocí volání usleep náhodný čas v intervalu <0,TZ>
   
    if(shm->open==0){                              // Pokud je pošta uzavřena
        print(f, "Z %d: going home\n", z_id, 0);   // Vypíše: A: Z idZ: going home 
        shm->zakaznik--;                           // Odeberte zákazníka z fronty, aby urednik pochopil, zda na poště někdo zůstal
        exit(0);                                   // Proces končí
    }else
    if(shm->open==1){                              // Pokud je pošta otevřená
   sem_wait(&shm->test3);
   if(shm->open==1){
    shm->number_service = rand() % 3 +1;           // Random 1-3
    print(f, "Z %d: entering office for a service %d\n", z_id, shm->number_service);    // Vypíše: A: Z idZ: entering office for a service X
    shm->count_service++; 
    if(shm->number_service==1) shm->count_1++;     // Zařadí se do fronty X
    else if(shm->number_service==2) shm->count_2++;
    else if (shm->number_service==3) shm->count_3++;
   
    sem_wait(&shm->zakaznik_waiting);                                                   // čeká na zavolání úředníkem                       
    print(f, "Z %d: called by office worker\n", z_id, 0);                               // Vypíše: Z idZ: called by office worker
    sleeping(10);                                                                       // Následně čeká pomocí volání usleep náhodný čas v intervalu <0,10>
   }
    print(f, "Z %d: going home\n", z_id, 0);                                            // Vypíše: A: Z idZ: going home
    shm->zakaznik--;                                                                    // Odeberte zákazníka z fronty, aby urednik pochopil, zda na poště někdo zůstal
    }
      sem_post(&shm->test3);
    exit(0);                                                                            // Proces končí
}
void Urednik(FILE *f,int u_id){
    srand(getpid() * time(NULL));
     print(f, "U %d: started\n", u_id, 0);
     while(shm->open==1 || shm->count_service>0 || shm->zakaznik>0 ){                                                // Když je pošta otevřena nebo služby nejsou dokončeny nebo na poště zůstávají zákazníci pracuji UREDNIKY [začátek cyklu]
        sem_post(&shm->zakaznik_waiting);                                                                            // Úředník jde obsloužit zákazníka z fronty X (vybere náhodně libovolnou neprázdnou).                                                                                         
        
        if(shm->count_service>0){                                                                                    // Services
        sem_wait(&shm->test);
        if(shm->count_1>0) { shm->count_1--;print(f, "U %d: serving a service of type 1\n", u_id, 0);sleeping(10);                                                                                                 // Následně čeká pomocí volání usleep náhodný čas v intervalu <0,10>
        print(f, "U %d: service finished\n", u_id, 0);                                                                // Vypíše: A: U idU: service finished
        shm->count_service--;}
        else if(shm->count_2>0) { shm->count_2--;print(f, "U %d: serving a service of type 2\n", u_id, 0);sleeping(10);                                                                                                 // Následně čeká pomocí volání usleep náhodný čas v intervalu <0,10>
        print(f, "U %d: service finished\n", u_id, 0);                                                                // Vypíše: A: U idU: service finished
        shm->count_service--;}
        else if (shm->count_3>0) { shm->count_3--;print(f, "U %d: serving a service of type 3\n", u_id, 0);sleeping(10);                                                                                                 // Následně čeká pomocí volání usleep náhodný čas v intervalu <0,10>
        print(f, "U %d: service finished\n", u_id, 0);                                                                // Vypíše: A: U idU: service finished
        shm->count_service--;}   
        sem_post(&shm->test);
        }
        sem_wait(&shm->test2);
         
         if (shm->count_service==0 && shm->open == 1)                                                                 // Pokud v žádné frontě nečeká zákazník a pošta je otevřená
         {
            print(f, "U %d: taking break\n", u_id, 0);                                                                // Vypíše: A: U idU: taking break
            sleeping(shm->tu);                                                                                        // Následně čeká pomocí volání usleep náhodný čas v intervalu <0,TU>
            print(f, "U %d: break finished\n", u_id, 0);                                                              // Vypíše: A: U idU: break finished
         }
         sem_post(&shm->test2);
         
        //Pokračuje na [začátek cyklu]

     }
     // Pokud v žádné frontě nečeká zákazník a pošta je zavřená
     print(f, "U %d: going home\n", u_id, 0);                                                                         // Vypíše: A: U idU: going home
     exit(0);                                                                                                         // Proces končí
     
    
}

int main(int argc, char **argv)
{
    
    // Number of arguments check
    if (argc != 6){
        fprintf(stderr, "chyba vstupu: spatny pocet argumentu \n");
        exit (1);
    }
    // Arguments Validation
    if (inputValidation(argv)) exit (1);

    FILE *f = open_file();
    setbuf(f, 0); 

    create_shared_memory();
    sem_initialize();
    set_value();

    shm->zakaznik = atoi(argv[1]);
    shm->nz = atoi(argv[1]);
    shm->nu = atoi(argv[2]); 
    shm->tz = atoi(argv[3]);
    shm->tu = atoi(argv[4]); 
    shm->f = atoi(argv[5]); 
    
    pid_t pidsU[shm->nu];
    pid_t pidsZ[shm->nz];
    
    for (int i = 0; i < shm->nu; i++){
       
        pidsU[i] = fork();
        
        if(pidsU[i] == 0){
            Urednik(f,(i+1));
        } else if(pidsU[i] == -1){
            error("ERROR!\nfork failed\n",EXIT_FAILURE);
        }  
    }

    for (int i = 0; i < shm->nz; i++){
       
        pidsZ[i] = fork();
        
        
        if(pidsZ[i] == 0){
            
            Zakaznik(f,(i+1));
        } else if(pidsZ[i] == -1){
            error("ERROR!\nfork failed\n",EXIT_FAILURE);
        }
    }
   

    sleepingF(shm->f);                                      // Čeká pomocí volání usleep náhodný čas v intervalu <F/2,F>
    shm->open = 0;
    print(f, "closing\n", 0,0);                             // Vypíše: A: closing
  
    
    while(wait(NULL) > 0);  
    delete_shared_memory();
    fclose(f);
    exit(0);
    return 0;

}