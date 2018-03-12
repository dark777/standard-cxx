#include <iostream>


char *chrncsnt(const char* str)
{
 char* char_not_const=(char*)str;
 //char* char_not_const=const_cast<char*>(str);
 return char_not_const;
}

const char *csntchr(char* str)
{
 const char* const_char=(const char*)str;
 //const char* const_char=const_cast<const char*>(str);
 return const_char;
}

int main()
{ 
 char *frase1;
 
 const char* minhafrase="Hello World";
 
 frase1 = chrncsnt(minhafrase);
 
 printf("\n\t%s\n\n",frase1);
 
 const char* frase2 = csntchr(frase1);
 
 printf("\n\t%s\n\n",frase2);
 return 0;
}