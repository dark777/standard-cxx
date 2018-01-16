#include "arv.hxx"

int main()
{
 int num;
 char Nome[100],op[2];
      
 Arvore *raiz;
 raiz = NULL;
      
 while(num != 6)
 {
  menu(&num);
  switch(num)
  {
    case 1:
         do{
            printf(
                   "\n\n\t*-------------------*"
                   "\n\t|  INSERT CONTACTS  |"
                   "\n\t*-------------------*\n"
                  );
           ler_string(Nome);
           inserir(&raiz,Nome);
           opcao(op);
          }while(*op == 'y' || *op =='Y');
           break;
    case 2:
           printf(
                  "\n\n\t*-----------------*"
                  "\n\t|  VIEW CONTACTS  |"
                  "\n\t*-----------------*\n"
                 );
           ordem(raiz);
           getchar();
           break;
    case 3:
           printf(
                  "\n\n\t*------------------*"
                  "\n\t|  SEARCH CONTACT  |"
                  "\n\t*------------------*\n"
                 );
           ler_string(Nome);
           busca(raiz,Nome);
           break;
    case 4:
         do{
            printf(
                   "\n\n\t*------------------*"
                   "\n\t|  REMOVE CONTACT  |"
                   "\n\t*------------------*\n"
                  );
           ler_string(Nome);
           opcao(op);
           if(*op == 'n' || *op =='N')break;
            excluir(&raiz,Nome);break;
           }while(*op =='s' || *op =='S');           
            break;
    case 5:
           printf(
                  "\n\n\t*-----------------*"
                  "\n\t|  ALTER CONTACT  |"
                  "\n\t*-----------------*\n"
                 );
           ler_string(Nome);
           alterar(&raiz,Nome);
           break;
    case 6:
           printf("\n\tGood Bye.\n\n");
           break;
  }
 }
}