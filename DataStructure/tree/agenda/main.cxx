#include "arv.hxx"

int main()
{
 int num;
 char Nome[50],op[2];
      
 Arvore *raiz = NULL;
      
 while(num != 6)
 {
  menu(&num);
  switch(num)
  {
    case 1:
         do{
           Menu_1();
           ler_string(Nome);
           inserir(&raiz,Nome);
           opcao(op);
          }while(*op == 's' || *op =='S');
           break;
    case 2:
           Menu_2();
           ordem(raiz);
           getchar();
           break;
    case 3:
           Menu_3();
           ler_string(Nome);
           busca(raiz,Nome);
           break;
    case 4:
         do{
           Menu_4();
           ler_string(Nome);
           opcao(op);
           if(*op == 'n' || *op =='N')break;
            excluir(&raiz,Nome);break;
           }while(*op =='s' || *op =='S');           
            break;
    case 5:
           Menu_5();
           ler_string(Nome);
           alterar(&raiz,Nome);
           break;
    case 6:
           printf("\n\tGood Bye.\n\n");
           break;
  }
 }
}