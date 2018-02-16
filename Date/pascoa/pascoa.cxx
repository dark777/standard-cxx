#include <iostream>

int main()
{
  time_t now = time(0);
  tm *ltm = localtime(&now);

  int anoatual(ltm->tm_year+1900);
  int mesatual(ltm->tm_mon+1);

  printf("\n\tO ano atual é %4d",anoatual);

  int ano;

  printf("\n\tDigite o ano desejado para\n\tcalcularmos o dia da páscoa: ");
  scanf("%d", &ano);

  int a=(ano%19);
  int b=int(ano/100);
  int c=(ano%100);
  int d=int(b/4);
  int e=(b%4);
  int f=int((b+8)/25);
  int g=int((b-f+1)/3);
  int h=((19*a+b-d-g+15)%30);
  int i=int(c/4);
  int k=int(c%4);
  int L=((32+2*e+2*i-h-k)%7);
  int m=int((a+11*h+22*L)/451);
  int mes=int((h+L-7*m+114)/31);

  const char* meses = "Mês inválido"; //aqui nova variável para o texto do mes

   switch(mes)
    { //agora com switch em vez de ifs
     case 1: meses = "Janeiro"; break;
     case 2: meses = "Fevereiro"; break;
     case 3: meses = "Março"; break;
     case 4: meses = "Abril"; break;
     case 5: meses = "Maio"; break;
     case 6: meses = "Junho"; break;
     case 7: meses = "Julho"; break;
     case 8: meses = "Agosto"; break;
     case 9: meses = "Setembro"; break;
     case 10: meses = "Outubro"; break;
     case 11: meses = "Novembro"; break;
     case 12: meses = "Dezembro"; break;
    }

 int dia=(((h+L-7*m+114)%31)+1);

 if(anoatual > ano || (ano == anoatual && mesatual > 4))
  printf("\n\tA pascoa caiu.!!\n\tdia: %d do mês: %s de %4d\n\n", dia, meses, ano);
 else
  printf("\n\tA pascoa ira cair.!!\n\tdia: %d do mês: %s de %4d\n\n", dia, meses, ano);
 
 return 0;
}