#include <stdio.h>
//este programa calcula a diferença em minutos de determinada hora inicial e final.
int mintotal(int hora, int minuto)
{
 int r = (hora*60) + minuto;
 return r;
}

int main()
{
 int h1, m1, h2, m2, r1, r2, total; //variaveis declaradas

 printf("\n\tDigite a hora:minuto inicial: ");
 scanf("%d:%d", &h1, &m1); //armazendando valores da hora inicial

 r1 = mintotal(h1, m1);
 
 printf("\n\tDigite a hora:minuto final: ");
 scanf("%d:%d", &h2, &m2);

 r2 = mintotal(h2, m2);

 total = r2-r1;

 if(total < 0)
  {
   total +=1440; //calculo
   
   printf("\n\tR1: %02d\n\tR2: %02d\n\t%d minutos de diferenca!\n\n", r1, r2, total); //impressão do resultado
  }
  else
  printf("\n\tR1: %02d\n\tR2: %02d\n\t%d minutos de diferenca!\n\n", r1, r2, total); //impressão do resultado
 return 0;
}