// Author: Myron Kukhta (xkukht01)
// project: IFJ2023 imperative language translator implementation
#include "param_list.h"

int param_list_init(Parametr_list_t** param_list){
    *param_list = (Parametr_list_t*)malloc(sizeof(Parametr_list_t));
    if(*param_list == NULL ) return ERROR_INTERNAL;
    (*param_list)->first=NULL;
    (*param_list)->last=NULL;
    (*param_list)->param_count=0;
    return OK;
}

void param_list_free(Parametr_list_t** param_list){
    if(*param_list != NULL){
        if(!is_empty(*param_list)){
            param_list_node_free(&((*param_list)->first));
        }
        free(*param_list);
        *param_list=NULL;
    }
}


int param_list_node_init(Parametr_list_node** new_node){
    *new_node = (Parametr_list_node*)malloc(sizeof(Parametr_list_node));
    if (*new_node != NULL){
        if(ds_init(&(*new_node)->parametr_name)) return ERROR_INTERNAL;
        (*new_node)->next_node=NULL;
        return OK;
    }
    return ERROR_INTERNAL;
}


void param_list_node_free(Parametr_list_node** node){
    if(*node!=NULL){
        if((*node)->next_node != NULL){
        param_list_node_free(&((*node)->next_node));
        }
        ds_free(&((*node)->parametr_name));
        free(*node);
        *node=NULL;
    }
}

int insert_new_param_to_list(Parametr_list_t* param_list, Dynamic_string_t* src_name, Data_type_IFJ scr_type){
    int return_code;
    if(is_empty(param_list))
        return_code = insert_first_node_to_list(param_list, src_name, scr_type);
    else{
        return_code = insert_after_node_to_list(param_list, src_name, scr_type);
    }
    if(!return_code)
        param_list->param_count+=1;
    return return_code;
}

int insert_first_node_to_list(Parametr_list_t* param_list, Dynamic_string_t* src_name, Data_type_IFJ scr_type){
    int return_code=0;
    if(!(return_code = param_list_node_init(&(param_list->first)))){
        if(!(return_code = insert_data_to_parametr(param_list->first, src_name, scr_type))){
            (param_list->first)->next_node=NULL;
            param_list->last = param_list->first;
        }
    }  
    return return_code;
}

int insert_after_node_to_list(Parametr_list_t* param_list, Dynamic_string_t* src_name, Data_type_IFJ scr_type){
    int return_code;
    Parametr_list_node** aux_node = &((param_list->last)->next_node);
    if(!(return_code = param_list_node_init(aux_node))){
        if(!(return_code = insert_data_to_parametr(*aux_node, src_name, scr_type))){
            (*aux_node)->next_node = NULL;
            param_list->last = (*aux_node);
        } 
    }
    return return_code;
}

bool is_empty(Parametr_list_t* param_list){
    if(param_list->last != NULL)
        return false;
    return true;
}
bool param_name_in_param_list(Parametr_list_t* param_list, Dynamic_string_t* target_name){
    Parametr_list_node* aux = param_list->first;
    if(!strcmp(target_name->str, UNDO)){
        return false;
    }
    while(aux!=NULL){
        if(!strcmp(aux->parametr_name->str, target_name->str)) return true;
        aux = aux->next_node;
    }
    return false;
}

int insert_data_to_parametr(Parametr_list_node* param, Dynamic_string_t* src_name, Data_type_IFJ scr_type){
    if(!ds_copy(src_name, param->parametr_name)){
        param->parametr_type = scr_type;
        return OK;
    }
    return ERROR_INTERNAL;
}