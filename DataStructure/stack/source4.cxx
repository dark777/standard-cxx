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
 * @file       source4
 * @version    0.1
 * @brief      Lista circular duplamente encadeada nao ordenada.
 * @consult    estruturas de dados algoritmos, análise da complexidade e implementações em java e cc++ - ana fernanda gomes ascencio & graziela santos araújo
 * @author     Jean Zonta
 * @Copyright (C) 2013-2017 Jean Zonta.
 * 
 * @end @section author Author
 *
*/
struct lista
{
 int op;
  int num;
  lista *prox;
 lista *ant;
};

int main()
{
 lista opt;
 lista *aux;
 lista *fim = NULL;
 lista *inicio = NULL;
 
 do{
    std::cout << "\n\n\tLISTA CIRCULAR DUPLAMENTE\n\tENCADEADA NAO ORDENADA"
                 "\n\tMENU DE ESCOLHA"
                 "\n\t[1]-INSERIR NO INICIO"
                 "\n\t[2]-INSERIR NO FIM"
                 "\n\t[3]-CONSULTAR TUDO" 
                 "\n\t[4]-CONSULTAR DO INICIO PARA FIM"
                 "\n\t[5]-CONSULTAR DO FIM PARA INICIO"
                 "\n\t[6]-REMOVER DA LISTA"
                 "\n\t[7]-ESVAZIAR A LISTA"
                 "\n\t[8]-SAIR"
                 "\n\tESCOLHA: ";
     std::cin >> opt.op;
     
    if(opt.op == 1)
     {
      std::cout << "\n\tQUANTOS NUMEROS DESEJA INSERIR NO INICIO?\n\tDIGITE: ";
      std::cin >> opt.num;
       
      for(int i = 0;i < opt.num; i++)
       {
        std::cout << "\n\tINSIRA NO INICIO O NUMERO " << i+1 << ": ";
        lista *novo = new lista();
        std::cin >> novo->num;
       
        if(inicio == NULL)
         {
          inicio = novo;
           fim = novo;
           novo->prox = inicio;
          novo->ant = inicio;
         }
        else
         {
          novo->prox = inicio;
           inicio->ant = novo;
           novo->ant = fim;
           fim->prox = novo;
          inicio = novo;
         }   
        std::cout << "\n\tNUMERO " << novo->num << " INSERIDO NO INICIO COM SUCESSO ..!!!\n";
       }
     }
     
    if(opt.op == 2)
     {
      std::cout << "\n\tQUANTOS NUMEROS DESEJA INSERIR NO FIM?\n\tDIGITE: ";
      std::cin >> opt.num;
       
      for(int i=0;i<opt.num;i++)
       {
        std::cout << "\n\tINSIRA NO FIM O NUMERO " << i+1 << ": ";
        lista *novo = new lista();
        std::cin >> novo->num;
      
        if(inicio == NULL)
         {
          inicio = novo;
           fim = novo;
           novo->prox = inicio;
          novo->ant = inicio;
         }
        else
         {
          fim->prox = novo;
           novo->ant = fim;
           fim = novo;
           fim->prox = inicio;
          inicio->ant = fim;
         }
        std::cout << "\n\tNUMERO " << novo->num << " INSERIDO NO FIM COM SUCESSO ..!!!\n";
       }
     }
     
    if(opt.op == 3)
     {
      if(inicio == NULL)std::cout << "\n\tLISTA VAZIA ..!!!\n";
      else
       {                                     // mostra a lista completa em caso de remoção no meio da lista
        std::cout << "\n\tLISTA COMPLETA: "; // EX: 1,2,3,4,5 inserido no inicio
        aux = inicio;                        // EX: 6,7,8,9,10 inserido no fim
        do{                                  // CONSULTA DO INICIO AO FIM: 5 4 3 2 1 6 7 8 9 10
           std::cout << aux->num << " ";     // CONSULTA DO FIM AO INICIO: 10 9 8 7 6 1 2 3 4 5
           aux = aux->prox;                  // REMOVA os elementos 9 e 2 ou 7 e 4
          }while(aux != inicio);             // LISTA COMPLETA: 5 4 3 1 6 7 8 10 
       }                                     
     }                                        
    
    if(opt.op == 4)
     {
      if(inicio == NULL)std::cout << "\n\tLISTA VAZIA ..!!!\n";
      else
       {
        std::cout << "\n\tCONSULTA DO INICIO AO FIM: ";
        aux = inicio;
         do{
            std::cout << aux->num << " ";
            aux = aux->prox;
           }while(aux != inicio);
       }
     }
     
    if(opt.op == 5)
     {
      if(inicio == NULL)std::cout << "\n\tLISTA VAZIA ..!!!!\n";
      else
      std::cout << "\n\tCONSULTA DO FIM AO INICIO: ";
      aux = fim;
      
      do{
         std::cout << aux->num << " ";
         aux = aux->ant;
        }while(aux != fim);
     }
  
    if(opt.op == 6)
     {
      int achou = 0;  
      int numero = 0;
      int quantidade = 0;      
      
      if(inicio == NULL)std::cout << "\n\tLISTA VAZIA ..!!!\n";
      else
       {
        std::cout << "\n\tREMOVA UM ELEMENTO INSERIDO: ";
        std::cin >> numero;
        aux = inicio;

        do{
           quantidade = quantidade + 1;
           aux = aux->prox;
          }while(aux != inicio);
       
        int elemento = 1;
        do{
           if(inicio == fim && inicio->num == numero)
            {
             inicio = NULL;
             delete(inicio);
             achou = achou + 1;
            }
           else
            {
             if(aux->num == numero)
              {
               achou = achou + 1;
                if(aux == inicio)
                 {
                  inicio = aux->prox;
                   inicio->ant = fim;
                   fim->prox = inicio;
                   delete(aux);
                  aux = inicio;
                 }
                else 
                if(aux == fim)
                 {
                  fim = fim->ant;
                   fim->prox = inicio;
                   inicio->ant = fim;
                   delete(aux);
                  aux = NULL;
                 }
                else
                 {
                  aux->ant->prox = aux->prox;
                   aux->prox->ant = aux->prox;
                   lista *aux2;
                   aux2 = aux;
                   aux = aux->prox;
                  delete(aux2);
                 }
              }
             else
             (aux=aux->prox);
            }
         elemento = elemento + 1;
        }while(elemento <= quantidade);

        if(achou == 0)
        std::cout << "\n\tNUMERO " << numero << " NÃO ENCONTRADO ..!!!\n";
         else
        std::cout << "\n\tNUMERO " << numero << " REMOVIDO " << achou << " VEZ ..!!!\n";
       }
     }
    
    if(opt.op == 7)
     {
      if(inicio == NULL)std::cout << "\n\tLISTA VAZIA ...!!!!\n";
       else
        {
         aux = inicio;  
         do{
            inicio = inicio->prox;
             delete(aux);
             aux = inicio;
           }while(aux != fim);

           delete(fim);
          inicio = NULL;
         std::cout << "\n\tLISTA ESVAZIADA ..!!!\n";
        }
     }
     
     if(opt.op < 1 || opt.op > 8)
     std::cout << "\n\tOPÇÃO INVÁLIDA!!!";
      else
     if(opt.op == 8)  
     std::cout << "\n\tGOOD BYE ...!!\n\n";
      
   }while(opt.op != 8);
 return 0;
}