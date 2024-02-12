CC = gcc
CFLAGS = -std=c99

all:
	$(CC) $(CFLAGS) -o main *.c 

pack:
	tar -cvzf xkukht01.tgz *.h *.c Makefile dokumentace.pdf rozdeleni

clean_pack:
	rm -f xkukht01.tgz

clean: clean_pack
	rm -rf *.o *.out