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

/* Calcular o dia de carnaval */
int diadocarnaval(int ano, int diapascoa, int mespascoa)
{
 int mes[12] = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
 
 mes[1] = (ano%4 == 0 || ano%400 == 0 && ano%100 != 0) ? 29 : 28; // atualiza dia+1 caso  ano seja bisexto
 
 if(mespascoa == 3)return int(mes[1]+diapascoa-47);
 else
 if(((diapascoa+31)-(mes[1]+diapascoa-47)) > 47)
  return int(47-(diapascoa+31));
 else
  return int(47-((31+diapascoa)+mes[1]));
}

/*Calcular o mes de carnaval*/
int mesdocarnaval(int diacarnaval, int diapascoa, int mespascoa)
{
 if(mespascoa == 3)return int(2);
 else
 if(((diapascoa+31)-diacarnaval) > 47)
  return int(3);
 else
  return int(2);
}

int mescorpus(int ano)
{
 int a=(ano%19);
 int b=(ano/100);
 int c=(ano%100);
 int d=(b/4);
 int e=(b%4);
 int f=((b+8)/25);
 int g=((b-f+1)/3);
 int h=((19 * a + b - d - g + 15)%30);
 int i=(c/4);
 int j=(c%4);
 int k=((32 + 2 * e + 2 * i - h - j)%7);
 int l=((a + 11 * h + 22 * k)/451);
 int mes=((h + k - 7 * l + 114)/31);
 int dia=(((h + k - 7 * l + 114)%31)+1);
 
 if(mes == 3)
 {
  if(((dia+60)-30) <= 30)
   return int(mes+1);
  else
   return int(mes+2);
 }
 else
 if(mes == 4)
 {
  if(((dia+60)-31) <= 31)
   return int(mes+1);
  else
   return int(mes+2);
 }
}

int diacorpus(int ano)
{
 int a=(ano%19);
 int b=(ano/100);
 int c=(ano%100);
 int d=(b/4);
 int e=(b%4);
 int f=((b+8)/25);
 int g=((b-f+1)/3);
 int h=((19 * a + b - d - g + 15)%30);
 int i=(c/4);
 int j=(c%4);
 int k=((32 + 2 * e + 2 * i - h - j)%7);
 int l=((a + 11 * h + 22 * k)/451);
 int mes=((h + k - 7 * l + 114)/31);
 int dia=(((h + k - 7 * l + 114)%31)+1);
 
 if(mes == 3)
 {
  if(((dia+60)-30) <= 30)
   return int((dia+60)-30);
  else
   return int(((dia+60)-30)-31);
 }
 else
 if(mes == 4)
 {
  if(((dia+60)-31) <= 31)
   return int(((dia+60)-31)+1);
  else
   return int(((dia+60)-31)-30);
 }
}

int main()
{
 int ano;
 
 printf("Qual o ano que quer consultar? ");
 scanf("%d", &ano);
 
 int dp = diadapascoa(ano); // dia pascoa
 
 int mp = mesdapascoa(ano); // mes pascoa
 
 int dc = diadocarnaval(ano,dp,mp); // dia carnaval
 
 int mc = mesdocarnaval(dc,dp,mp); // mes carnaval
 
 int dcc = diacorpus(ano); // dia corpus christ
 
 int mcc = mescorpus(ano); // mes corpus christ
 
 printf("\n\tCorpus Christ será no dia %02d do mes %02d do ano %4d.",dcc, mcc,ano);
 
 printf("\n\tA Páscoa será no dia %02d do mes %02d do ano %4d." ,dp,mp,ano);
 if(mc == 3)
 printf("\n\tO carnaval será no dia %02d do mes %02d do ano %4d.\n\n",((2-dc)-2),mc,ano);
 else
 printf("\n\tO carnaval será no dia %02d do mes %02d do ano %4d.\n\n",(dc-2),mc,ano);
 
 return 0;
}