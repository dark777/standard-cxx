#include <stdio.h>





int main()
{
 int ano;
 
 printf("Digite um ano: ");
 scanf("%d",&ano);
 
 //calcula numero aureo
 int a = (ano % 19)+1;
 
 //seculo
 int b = (ano/100)+1;
 
 //calcula as correcoes    
 int c = ((3*b)/4)-12;
 int d = (((8*b)+5)/25)-5;
 
 //epacta
 int e = ((11*a)+ 20 + d - c)%30;
 
 if((e == 25 && a > 11) || (e == 24))e++;
 
 //calcular lua cheia
 int f = (44-e);
     f = (f<21) ? f = f+30 : f;
 
 //calcula domingo
 int g = ((5*ano)/4)-(c+10);

 f = (f+7)-((g+f)%7);
    
 if(f > 31)
  printf("\n\tPascoa: %d de April de %d\n\n", (f-31), ano);
 else
  printf("\n\tPascoa: %d de March de %d\n\n", f, ano);
 
 return 0;
}