#include <cstring>
#include <iostream>
/*
Programinha em C++ que checa se um número de CPF é válido

validacpf1.cpp - feb/2018 - Jean Zonta

Modificação do Script Referência: 
 https://www.vivaolinux.com.br/script/Validar-CPF-em-C-esse-funciona
 https://www.vivaolinux.com.br/script/Formatar-strings-em-C-(RG-telefone-CEP-etc)/
*/

void  getCpf();
char* fmtCpf(char *, const char *);


int main()
{
 getCpf();
 std::cout<<"\n";
 return 0;
}


struct validation
{
 validation(char *input): _input(input)
 {
  for(char i = 0; i < 11; i++)
   {
    //Convertendo char para valor absoluto segundo tabela ASCII e passando para array de inteiros// 
    cpf[i] = static_cast<int>(input[i] - 48);
    
    //Validando a entrada de dados
    if(cpf[i] < 0 || cpf[i] > 9)std::cout << "\n\tENTRADA INVÁLIDA\n";
   }
    
  for(char i = 0; i < 11; i++)
   {
    cpf[i];
    if(i == 2 || i == 5);
    if(i == 8);
   }
 }

 bool isCpf()
 {
  int digito1;
  int digito2;
  int temp = 0;
  const int *cpf;
    /*Obtendo o primeiro digito verificador:
  
    Os 9 primeiros algarismos são multiplicados pela sequência 10, 9, 8, 7, 6, 5, 4, 3, 2
    (o primeiro por 10, o segundo por 9, e assim por diante);
    Em seguida, calcula-se o resto “r1″ da divisão da soma dos resultados das multiplicações por 11,
    e se o resto for zero ou 1, digito é zero, caso contrário digito = (11-r1) */
    
    for(char i = 0; i < 9; i++)
     temp += (cpf[i] * (10 - i));
      temp %= 11;
    
    if(temp < 2)
     digito1 = 0;
      else
       digito1 = 11 - temp;
    
    /*Obtendo o segundo digito verificador:
    O dígito2 é calculado pela mesma regra, porém inclui-se o primeiro digito verificador ao final
    da sequencia. Os 10 primeiros algarismos são multiplicados pela sequencia 11, 10, 9, ... etc...
    (o primeiro por 11, o segundo por 10, e assim por diante);
    procedendo da mesma maneira do primeiro digito*/
    
    temp = 0;
     for(char i = 0; i < 10; i++)
      temp += (cpf[i] * (11 - i));
       temp %= 11;
    
    if(temp < 2)
     digito2 = 0;
      else
       digito2 = 11 - temp;
    
    /* Se os digitos verificadores obtidos forem iguais aos informados pelo usuário,
       então o CPF é válido */
    
    if(digito1 == cpf[9] && digito2 == cpf[10])
     return true;
      else
       return false;
  }
  
  validation* print()
  {
   std::cout<<"\n\tCpf: "<<fmtCpf(_input,"###.###.###-##")<<(validation(_input).isCpf()?" is Valid\n":" is Invalid\n");  
  }
  
  private:
   int cpf[11];
   char *_input;
};

char* fmtCpf(char *frase, const char *fmt)
{
 int i = 0; 
 char aux[100];
 
  while(*frase)
   {
    if(fmt[i] != '#')
     {
      aux[i] = fmt[i];
      i++;
     }
     else
     {
      aux[i] = *frase;
      frase++;
      i++;
     }
   }
  aux[i] = 0;
 strcpy(frase, aux);
}

void getCpf()
{
 char input[12];
 
 do{
    std::cout << "\n\tInforme o CPF sem pontos, espaços ou traços\n\tDigite: ";
    std::cin.getline(input, 12, '\n');
    
    validation(input).print();
    
   }while(validation(input).isCpf() == 0);
}