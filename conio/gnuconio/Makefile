CC=gcc
CXX=g++
AR=ar
RANLIB=ranlib

all: conio_test.exe conio_test_unicode.exe constream_test.exe

conio_test.exe: conio_test.c libconio.a
	$(CC) -Wall -O3 -s conio_test.c -o conio_test.exe -lconio -L. -I.

conio_test_unicode.exe: conio_test.c libconio_unicode.a
	$(CC) -Wall -O3 -s conio_test.c -o conio_test_unicode.exe -lconio_unicode -L. -I. -DUNICODE -lmsvcr71

constream_test.exe: constream_test.cpp constream libconio.a
	$(CXX) -Wall -O3 -s constream_test.cpp -o constream_test.exe -lconio -L. -I.

libconio.a: conio.o
	$(AR) r libconio.a conio.o
	$(RANLIB) libconio.a

libconio_unicode.a: conio_unicode.o
	$(AR) r libconio_unicode.a conio_unicode.o
	$(RANLIB) libconio_unicode.a

conio.o: conio.c conio.h
	$(CC) -Wall -O3 -s -c conio.c -o conio.o

conio_unicode.o: conio.c conio.h
	$(CC) -Wall -O3 -s -c conio.c -o conio_unicode.o -DUNICODE

clean:
	rm -f *.o libconio.a libconio_unicode.a *.exe
