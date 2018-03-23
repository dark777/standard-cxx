#include <iostream>



int main()
{
 int dia;
 int mes;
 int ano;
 
 do{
    //O Metodo de carter só permite calcular a data
    //da pascoa entre os anos de 1900 e 2099
    std::cout << "\n\tdigite um ano entre 1900 e 2099: ";
    std::cin >> ano;
    
    if(ano < 1900 || ano > 2099)std::cout<<"\n\tano invalido!\n\n";
      
   }while(ano < 1900 || ano > 2099);
  
  int a = (255 - (11 * ( ano % 19 )));
  int b = (((a - 21) % 30) + 21);
  
  if(b > 38)b=(b-1);
  
  int c = (( ano + (ano / 4) + b + 1) % 7);
  int d = ((b + 7) - c);
  
  if(d < 32)
   {
    dia = d;
    mes = 3;
   }
   else
   {
    dia = (d - 31);
    mes = 4;
   }
   
  printf("\n\tO dia da pascoa é: %02d/%02d/%4d\n\n",dia,mes,ano);
 return 0;
}