#include <iostream>
/*!
 *
 * @begin @section terms_of_use Terms of Use
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * @end @section terms_of_use Terms of Use
 * 
 * @begin @section author Author
 * 
 * @file       source1
 * @version    0.1
 * @brief      Algoritmos de Busca Sequencial Ordenada e nao Ordenada.
 * @consult    estruturas de dados algoritmos, análise da complexidade e implementações em java e cc++ - ana fernanda gomes ascencio & graziela santos araújo.pdf
 * @author     Jean Zonta
 * @Copyright (C) 2013 Jean Zonta.
 * 
 * @end @section author Author
 *
*/
int main()
{
 int i, n, op, achou, X[10];

       while (op != 3)
       {
        std::cout << "\n\tALGORITMOS DE BUSCA SEQUENCIAL"
                     "\n\tORDENADA E NAO ORDENADA!!!\n"
                     "\n\tMENU DE ESCOLHA ..!!!!"
                     "\n\t[1] - NUMEROS NAO ORDENADOS "
                     "\n\t[2] - NUMEROS ORDENADOS "
                     "\n\t[3] - SAIR"
                     "\n\tOP: ";
        std::cin >> op;
         
         if(op == 1)
          {
            std::cout << "\n\tVETOR COM NUMEROS NAO ORDENADOS!!!!\n";
              
              for(i = 0; i <= 9; i++)
                {
                 std::cout << "\n\tDIGITE O " << i + 1 << " NUMERO: ";
                 std::cin >> X[i];
                }
                
              do{
                 std::cout << "\n\tBUSQUE UM NUMERO NO VETOR: ";
                 std::cin >> n;

                 achou = 0;
                 i = 0;
                 
                  while(i <= 9 && achou == 0)
                   {
                     if(X[i] == n)achou = 1;
                      else
                     i++;
                   }
                   
                    if(achou == 0)std::cout << "\n\tNUMERO NAO ENCONTRADO!!!!\n\t";
                     else
                    std::cout << "\n\tNUMERO ENCONTRADO NA POSICAO: " << i + 1 << "\n\t";
                }while(!achou);
          }else
          
         if(op == 2)
          {
            std::cout << "\n\tVETOR COM NUMEROS ORDENADOS!!!!\n";

              for(i = 0; i <= 9; i++)
                {
                 std::cout << "\n\tDIGITE O " << i + 1 << " NUMERO: ";
                 std::cin >> X[i];
                }
                
              do{
                 std::cout << "\n\tBUSQUE UM NUMERO NO VETOR: ";
                 std::cin >> n;

                 achou = 0;
                 i = 0;

                  while(i <= 9 && achou == 0 && n >= X[i])
                   {
                     if(X[i] == n)achou = 1;
                      else
                     i++;
                   }
                   
                    if(achou == 0)std::cout << "\n\tNUMERO NAO ENCONTRADO!!!!\n\t";
                     else
                    std::cout << "\n\tNUMERO ENCONTRADO NA POSICAO: " << i + 1 << "\n\t";
                }while(!achou);
            }
           if(op != 3)
           std::cout << "\n\tOPCAO INVALIDA ..!!!\n";
            else
           std::cout << "\n\tGOOD BYE ...!!\n\n";
          }
  return 0;       
}