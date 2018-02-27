#include <iostream>
//http://www.webcid.com.br/calendario/2018/brasil

int main()
{

time_t now = time(0);
tm *ltm = localtime(&now);

int anoatual = ltm->tm_year+1900;


printf("O ano atual é %4d.", anoatual);

int ano;

printf("Digite o ano desejado para calcularmos o dia da páscoa: ");
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
int k=(c%4);

int L=((32+2*e+2*i-h-k)%7);
int m=int((a+11*h+22*L)/451);

int mes=int((h+L-7*m+114)/31); //esta ainda igual também

char* mes1 = "Mês inválido"; //aqui nova variável para o texto do mes

switch (mes){ //agora com switch em vez de ifs
    case 1: mes1 = "Janeiro"; break;
    case 2: mes1 = "Fevereiro"; break;
    case 3: mes1 = "Março"; break;
    case 4: mes1 = "Abril"; break;
    case 5: mes1 = "Maio"; break;
    case 6: mes1 = "Junho"; break;
    case 7: mes1 = "Julho"; break;
    case 8: mes1 = "Agosto"; break;
    case 9: mes1 = "Setembro"; break;
    case 10: mes1 = "Outubro"; break;
    case 11: mes1 = "Novembro"; break;
    case 12: mes1 = "Dezembro"; break;
}

int dia=((h+L-7*m+114)%31)+1;

if (anoatual>ano) { //printfs agora sem &
    printf("A pascoa caiu no dia: %d.",dia);
    printf("Do mês: %s", mes1);
}
else if (anoatual<ano) { //printfs agora sem &
    printf("A pascoa ira cair no dia: %d.",dia);
    printf("Do mês: %s", mes1);
}

 return 0;
}