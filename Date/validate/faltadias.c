#include <stdio.h>
#include <stdlib.h>

//http://algoritmosurgentes.com/algoritmo.php?a=39
//http://algoritmosurgentes.com/algoritmo.php?a=46












int main ()
{
 int falta_dias = 0;
 int dia, mes, ano;
 int dias_mes[12] = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
 
 do{
    printf("\n\tEnter date format dd/mm/yyyy: ");
    scanf("%d/%d/%d", &dia, &mes, &ano);
    
    if(dia > 31 || dia < 1)printf("\n\tDay %d is Invalid.!!\n\tEnter day between 01 and 31\n",dia);
    
    if(mes > 12 || mes < 1)printf("\n\tMes %d is Invalid.!!\n\tEnter day between 01 and 12\n",mes);
    
    if(ano%4 == 0 || ano%400 == 0 && ano%100 != 0)dias_mes[1]=29; // atualiza dia+1 caso  ano seja bisexto
    
    if(dia > dias_mes[mes-1])printf("\n\tMes %d of year %d does not have %d days!!!\n\n",mes,ano,dia);
    
   }while((dia > 31 || dia < 1) || (mes > 12 || mes < 1) || (dia > dias_mes[mes-1]));
   
   dias_mes[1] = (ano%4 == 0 || ano%400 == 0 && ano%100 != 0) ? 29 : 28;
   
   for(int i = mes; i<12; i++)
   falta_dias+=dias_mes[i];
   
   falta_dias+=dias_mes[mes-1]-dia; // conta os dias restantes do mes indicado na entrada padrão
   
   printf("\n\tDays %d to complete the year %04d.\n\n", falta_dias,ano);
   
 return 0;
}