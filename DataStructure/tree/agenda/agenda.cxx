/*
  Autor: Marcos Augusto
  Email: marcosccp04@gmail.com
  Modified: Jean Zonta
  Email: wiki.anon@yahoo.com.br
*/
#include "arv.hxx"

int main()
{
 int num;
 char op[2];
 char Nome[100];
      
 Arvore *raiz = NULL;
 
 menu(&num);
 
 while(num != 6) 
 {
  switch(num)
  {
    case 1:
         do{
            printf(
                   "\n\n\t*-------------------*"
                   "\n\t|  INSERT CONTACTS  |"
                   "\n\t*-------------------*\n"
                  );
           getChars(Nome);
           inserir(&raiz,Nome);
           printf("\n\tDo you want to continue (y)es or (n)o: ");
           scanf(" %s",&op);
           if(*op == 'n' || *op =='N')menu(&num);
          }while(*op == 'y' || *op =='Y');
           break;
    case 2:
           printf(
                  "\n\n\t*-----------------*"
                  "\n\t|  VIEW CONTACTS  |"
                  "\n\t*-----------------*"
                 );
           if(raiz != NULL)ordem(raiz);
           menu(&num);
           break;
    case 3:
          do{
           printf(
                  "\n\n\t*------------------*"
                  "\n\t|  SEARCH CONTACT  |"
                  "\n\t*------------------*\n"
                 );
           getChars(Nome);
           busca(raiz,Nome);
           printf("\n\tDo you want to continue (y)es or (n)o: ");
           scanf(" %s",&op);
           if(*op == 'n' || *op =='N')menu(&num);
           else
           break;
           }while(*op =='y' || *op =='Y'); 
           break;
    case 4:
         do{
            printf(
                   "\n\n\t*------------------*"
                   "\n\t|  REMOVE CONTACT  |"
                   "\n\t*------------------*\n"
                  );
           getChars(Nome);
           printf("\n\tDo you want to continue (y)es or (n)o: ");
           scanf(" %s",&op);
           if(*op == 'n' || *op =='N')menu(&num);
            else
           excluir(&raiz,Nome);
           }while(*op =='y' || *op =='Y');            
           break;
    case 5:
           do{
           printf(
                  "\n\n\t*-----------------*"
                  "\n\t|  ALTER CONTACT  |"
                  "\n\t*-----------------*\n"
                 );
           getChars(Nome);
           busca(raiz,Nome);
           printf("\n\tDo you want to continue (y)es or (n)o: ");
           scanf(" %s",&op);
           if(*op =='n' || *op =='N')menu(&num);
	   else
           {
            alterar(&raiz,Nome);
            menu(&num);
            break;
           }
           }while(*op =='y' || *op =='Y');   
           break;
    case 6:
           printf("\n\tGood Bye.\n\n");
           break;   
  };
 }
 return 0;
}