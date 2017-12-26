#ifndef _IOSTR_C
#define _IOSTR_C

#include <StrSet/include/strset.h>
#include <StrSet/src/rdspc/rdspc.c>

void iostr(char* string)
{
 fgets(string,1000,stdin);
   char *lstring = rdspc(string);
   printf("\t%s\n",lstring);
 free(lstring);  
}

#endif
