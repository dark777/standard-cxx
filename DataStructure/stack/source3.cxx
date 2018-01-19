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
 * @brief      Lista Circular Simples Nao Ordenada.
 * @consult    estruturas de dados algoritmos, análise da complexidade e implementações em java e cc++ - ana fernanda gomes ascencio & graziela santos araújo.pdf
 * @author     Jean Zonta
 * @Copyright (C) 2013 Jean Zonta.
 * 
 * @end @section author Author
 *
*/
struct lista
{
 int num;
 lista *prox;
};

int main()
{
 lista *inicio = NULL;
 lista *fim = NULL;
 lista *aux;
 lista *ant;
 int op, numero, achou;

 do{
    std::cout<<"\n\n\tLISTA CIRCULAR SIMPLES NÃO ORDENADA"
               "\n\tMENU DE ESCOLHA DA LISTA"
               "\n\t1 - INSERIR NO INICIO"
               "\n\t2 - ISERIR NO FIM"
               "\n\t3 - CONSULTAR TUDO"
               "\n\t4 - REMOVER"
               "\n\t5 - ESVAZIAR"
               "\n\t6 - SAIR"
               "\n\tESCOLHA: ";
   std::cin >> op;
    
      if(op == 1)
       {
        printf("\n\tINSIRA NUMERO NO INICIO: ");
        lista *novo = new lista();
        std::cin >> novo->num;

        if(inicio == NULL)
         {
          inicio = novo;
          fim = novo;
          fim->prox = inicio;
         }
        else
         {
          novo->prox = inicio;
          inicio = novo;
          fim->prox = inicio;
         }
        printf("\n\tINSERIDO COM SUCESSO!!!");
       }
       
      if(op == 2)
       {
        printf("\n\tINSIRA NO FIM: ");
        lista *novo = new lista();
        std::cin >> novo->num;
        
        if(inicio == NULL)
        {
         inicio = novo;
         fim = novo;
         fim->prox = inicio;
        }
       else
        {
         fim->prox = novo;
         fim = novo;
         fim->prox = inicio;
        }
         printf("\n\tINSERIDO NO FIM COM SUCESSO!!!");
       }
       
      if(op == 3)
       {
        if(inicio == NULL)
         {
          printf("\n\tLISTA VAZIA!!!");
         }
        else
         {
          printf("\n\tLISTA COMPLETA: ");
          aux = inicio;
           do
            {
             std::cout << aux->num << " ";
             aux = aux->prox;
            }while(aux != inicio);
         }
       }
       
      if(op == 4)
       {
        if(inicio == NULL)
         {
          printf("\n\tLISTA VAZIA!!!");
         }
        else
         {
          printf("\n\tREMOVA UM ELEMENTO: ");
          std::cin >> numero;
          aux = inicio;
          ant = NULL;
          achou = 0;
          int quantidade = 0;
          aux = inicio;
          do
           {
            quantidade = quantidade + 1;
            aux = aux->prox;
           }while(aux != inicio);

          int elemento = 1;
          do
           {
            if(inicio == fim && inicio->num == numero)
             {
              delete(inicio);
              inicio = NULL;
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
                  fim->prox = inicio;
                  delete(aux);
                  aux = inicio;
                 }
                else
                if(aux == fim)
                 {
                  fim = ant;
                  fim->prox = inicio;
                  delete(aux);
                  aux = NULL;
                 }
                else
                 {
                  ant->prox = aux->prox;
                  delete(aux);
                  aux = ant->prox;
                 }
               }
              else
               {
                ant = aux;
                aux = aux->prox;
               }
             }
              elemento = elemento + 1;
           }while(elemento <= quantidade);
           
           if(achou == 0)printf("\n\tNUMERO NAO ENCONTRADO!!!");
           else
           if(achou == 1)printf("\n\tNUMERO REMOVIDO 1 VEZ!!!");
           else
           std::cout<<"\n\tNUMERO REMOVIDO " <<achou<< " ";
         }
       }
       
      if(op == 5)
       {
        if(inicio == NULL)std::cout << "\n\tLISTA VAZIA!!!";
        else
         {
          aux = inicio;
           do
             {
              inicio = inicio->prox;
              delete(aux);
              aux = inicio;
             }while(aux != fim);
            delete(fim);
           inicio = NULL;
          std::cout<<"\n\tLISTA ESVAZIADA COM SUCESSO ..!!!\n";
         }
       }
       
      if(op != 6)
      std::cout << "\n\tOPCAO INVALIDA ..!!!\n";
       else
      std::cout << "\n\tGOOD BYE ..!!!\n";
   
   }while(op != 6);
 return 0;
}