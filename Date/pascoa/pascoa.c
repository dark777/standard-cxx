#include <stdio.h>

int main()
{
 int ano;
 
 printf("Digite um ano: ");
 scanf("%d",&ano);
 
 //calcula numero aureo
 int g = (ano % 19)+1;
 
 //seculo
 int c = (ano/100)+1;
 
 //calcula as correcoes    
 int x = ((3*c)/4)-12;
 int z = (((8*c)+5)/25)-5;
 
 //epacta
 int e = ((11*g)+ 20 + z - x)%30;
 
 if((e == 25 && g > 11) || (e == 24))e++;
 
 //calcular lua cheia
 int n = (44-e);
     n = (n<21) ? n = n+30 : n;
 //calcula domingo
 int d = ((5*ano)/4)-(x+10);

 n = (n+7)-((d+n)%7);
    
 if(n > 31)
 {
  int DiaPascoaAbril = (n-31);
  printf("\n\tPascoa: %d de April de %d\n\n", (n-31), ano);
 }
 else
 {
  int DiaPascoaMarco = n;
  printf("\n\tPascoa: %d de March de %d\n\n", n, ano);
 }
 return 0;
}