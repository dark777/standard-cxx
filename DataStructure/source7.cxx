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
 * @file       source7
 * @version    0.1
 * @brief      Pilha Desempliha como estrutura dinamica.
 * @consult    estruturas de dados algoritmos, análise da complexidade e implementações em java e cc++ - ana fernanda gomes ascencio & graziela santos araújo.pdf
 * @author     Jean Zonta
 * @Copyright (C) 2013 Jean Zonta.
 * 
 * @end @section author Author
 *
 * PS: para desemplihar é preciso remover o ultimo elemento inserido no topo da pilha.
 */
struct pilha
{
 int num;
 pilha *prox;
};

int main()
{
 pilha *topo = NULL;
 pilha *aux;
 int op;
  do
   {
    std::cout << "\n\n\tPILHA COMO ESTRUTURA DINAMICA"
                 "\n\tMENU DE ESCOLHA DA PILHA"
                 "\n\t1 - INSERIR NA PILHA"
                 "\n\t2 - CONSULTAR TODA PILHA"
                 "\n\t3 - REMOVER DA PILHA"
                 "\n\t4 - DESEMPILHAR A PILIHA"
                 "\n\t5 - ESVAZIAR A PILHA" 
                 "\n\t6 - SAIR"
                 "\n\tESCOLHA: ";
    std::cin >> op;
    
       if(op == 1)
        {
         std::cout << "\n\tINSIRA NUMERO NA PILHA: ";
         pilha *novo = new pilha();
         std::cin >> novo->num;
         novo->prox = topo;
         topo = novo;
         std::cout << "\n\tNUMERO "<<novo->num<<" INSERIDO COM SUCESSO!!!";
        }
        
       if(op == 2)
        {
         if(topo == NULL)std::cout << "\n\tPILHA VAZIA!!!";
         else
          {
           std::cout << "\n\tPILHA COMPLETA: ";
           aux = topo;
           while(aux != NULL)
           {
            std::cout << aux->num << " ";
            aux = aux->prox;
           }
          }
        }
        
       if(op == 3)
        {
         if(topo == NULL)std::cout << "\n\tPILHA VAZIA!!!";
         else
          {
           aux = topo;
           std::cout << "\n\tNUMERO: " << topo->num << " REMOVIDO COM SUCESSO!!!";
           topo = topo->prox;
           delete(aux);
          }
        }
                
       if(op == 4)
        {
         if(topo == NULL)std::cout << "\n\tPILHA VAZIA!!!";
         else
          {
           std::cout<<"\n\tTOPO: "<<topo->num;
           aux = topo->prox;
	   
           while(aux != NULL)
           {
            aux = aux->prox;
            topo->prox--;
           }
          }
        }
        
       if(op == 5)
        {
         if(topo == NULL)std::cout << "\n\tPILHA VAZIA!!!";
         else
          {
           aux = topo;
           while(aux != NULL)
           {
            topo = topo->prox;
            delete(aux);
            aux = topo;
           }
           std::cout << "\n\tPILHA ESVAZIADA COM SUCESSO!!!!";
          }
        }
        
       if(op<1 || op>6)std::cout << "\n\tOPCAO INVALIDA!!!";
       else
       if(op == 6)std::cout<<"\n\tGOOD BYE ...!!\n\n";

   }while(op != 6);
 return 0;
}