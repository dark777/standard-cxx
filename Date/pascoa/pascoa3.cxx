#include <iostream>

/*Calcular o mes da pascoa*/
int mesdapascoa(int ano)
{
 int a=(ano/100);
 int b=(ano-(19*(ano/19)));
 int c=((a-17)/25);
 int d=(((a-(a/4))-((a-c)/3))+((19*b)+15));
 int e=(d-(30*(d/30)));
 int f=(e-((e/28)*(1-(e/28))*(29/(e+1))*((21-b)/11)));
 int g=((ano+(ano/4))+(((f+2)-a)+(a/4)));
 int h=(g-(7*(g/7)));
 int i=(f-h);
 
 return int(3+((i+40)/44));
}

/* Calcular o dia da páscoa*/
int diadapascoa(int ano)
{
 int a=(ano/100);
 int b=(ano-(19*(ano/19)));
 int c=((a-17)/25);
 int d=(((a-(a/4))-((a-c)/3))+((19*b)+15));
 int e=(d-(30*(d/30)));
 int f=(e-((e/28)*(1-(e/28))*(29/(e+1))*((21-b)/11)));
 int g=((ano+(ano/4))+(((f+2)-a)+(a/4)));
 int h=(g-(7*(g/7)));
 int i=(f-h);
 int j=(3+((i+40)/44));
 
 return int(((i+28)-(31*(j/4))));
}
 
/* Calcular a quantidade de dias dos meses*/
int dias_mes(int ano)
{
 int diasmes[12] = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
 
 return diasmes[1] = (ano%4 == 0 || ano%400 == 0 && ano%100 != 0) ? 29 : 28; // atualiza dia+1 caso  ano seja bisexto
}

/* Calcular o dia de carnaval */
int diadocarnaval (int ano, int mes[12], int diapascoa, int mespascoa)
{
 int diacarnaval2;
 int diacarnaval;
 
 diacarnaval2 = mes[2]+diapascoa-47;
 
 if(mespascoa == 3)
  diacarnaval = mes[2]+diapascoa-47;
 else
 if(diapascoa+31-diacarnaval2 > 47)
  diacarnaval = (47-(diapascoa+31));
 else
  diacarnaval = (47-((31+diapascoa)+mes[2]));
 
 return diacarnaval;
}

/*Calcular o mes de carnaval*/
int mesdocarnaval(int ano, int diacarnaval, int diapascoa, int mespascoa)
{
 int mescarnaval;
 
 if(mespascoa == 3)mescarnaval = 2;
 else
 
 if(((diapascoa+31)-diacarnaval) > 47)
  mescarnaval = 3;
 else
  mescarnaval = 2;
 
 return mescarnaval;
}

int main()
{
 int ano;
 int dp;
 int mp;
 int mc;
 int dc;
 int dmc[12];
 
 printf("Qual o ano que quer consultar? ");
 scanf("%d" ,&ano);
 
 mp=mesdapascoa(ano);
 
 dp=diadapascoa(ano);
 
 dmc[2]=dias_mes(ano);
 
 dc=diadocarnaval(ano,dmc,dp,mp);
 
 mc=mesdocarnaval(ano,dc,dp,mp);
 
 printf("\n\tA Páscoa será no dia %d do mes %d do ano %d." ,dp,mp,ano);
 if(mc == 3)
 printf("\n\tO carnaval será no dia %d do mes %d do ano %d.\n\n",((2-dc)-2),mc,ano);
 else
 printf("\n\tO carnaval será no dia %d do mes %d do ano %d.\n\n",(dc-2),mc,ano);
 
 return 0;
}