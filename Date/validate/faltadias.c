#include <stdio.h>
#include <stdlib.h>

//http://algoritmosurgentes.com/algoritmo.php?a=39
//http://algoritmosurgentes.com/algoritmo.php?a=46












int main ()
{
 int falta_dias = 0;
 int dia, mes, ano;
 int dias_mes[13] = {0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
 
 do{
    printf("Digite uma data no formato dd/mm/yyyy: ");
    scanf("%d/%d/%d", &dia, &mes, &ano);
    
    if(dia>31 || dia<1)printf("\n\tDia %d invalido.!!\n\tDigite um dia de 01 a 31\n",dia);
    
    if(mes>12 || mes <1)printf("\n\tMes %d invalido.!!\n\tDigite um mes de 01 a 12\n",mes);
    
   }while((dia>31 || dia<1) || (mes>12 || mes <1));
   
   dias_mes[2] = (ano%4 == 0 || ano%400 == 0 && ano%100 != 0) ? 29 : 28;
   
   for(int i = mes; i<13; i++)
   falta_dias += dias_mes[i];
    
   printf("\n\nFaltam %d dias para terminar o ano %04d.\n\n", falta_dias,ano);
   
 return 0;
}