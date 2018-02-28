#include <iostream>

//https://giovanibettiol.wordpress.com/2009/10/06/algoritmo-para-calcular-o-periodo-do-horario-de-verao/
int domingoDePascoa(int ano)
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
 
 return dia;
 
 //return new dateTime(ano, mes, dia);
}

int domingoDeCarnaval(int ano)
{
 return domingoDePascoa(ano)-49;
}

//Retorna a data de início do horário de verão de um determinado ano
int InicioHorarioVerao(int ano)
{
// terceiro domingo de outubro
int primeiroDeOutubro = EncodeDate((Word)ano, 10, 1);
int primeiroDomingoDeOutubro = primeiroDeOutubro + ((7 – primeiroDeOutubro.DayOfWeek()+1) % 7);
int terceiroDomingoDeOutubro = primeiroDomingoDeOutubro + 14;
return terceiroDomingoDeOutubro;
}


//Retorna a data de término do horário de verão de um determinado ano
int erminoHorarioVerao(int ano)
{
int primeiroDeFevereiro = EncodeDate((Word)ano + 1, 2, 1);
int primeiroDomingoDeFevereiro = primeiroDeFevereiro + ((7 – primeiroDeFevereiro.DayOfWeek()+1) % 7);
int terceiroDomingoDeFevereiro = primeiroDomingoDeFevereiro + 14;

if (terceiroDomingoDeFevereiro != DomingoDeCarnaval(ano+1))
{
return terceiroDomingoDeFevereiro;
}
else
{
return terceiroDomingoDeFevereiro + 7;
}

}

int main()
{
  return 0;
}