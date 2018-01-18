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
           if(*op == 'n' || *op == 'N')menu(&num);
          }while(*op == 'y' || *op == 'Y');
           break;
    case 2:
           printf(
                  "\n\n\t*-----------------*"
                  "\n\t|  VIEW CONTACTS  |"
                  "\n\t*-----------------*\n"
                 );   
           if(raiz == NULL)
            {
             printf("\n\tContacts not found!!");
             printf("\n\n\tDo you want to continue (y)es or (n)o: ");
             scanf(" %s",&op);     
             if(*op == 'y' || *op == 'Y')menu(&num);
             else
             return 0;
           }
           else
           {
            ordem(raiz);
             printf("\n\n\tDo you want to continue (y)es or (n)o: ");
             scanf(" %s",&op);     
             if(*op == 'y' || *op == 'Y')menu(&num);
             else
             return 0;
           }
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
           printf("\n\n\tDo you want to continue (y)es or (n)o: ");
           scanf(" %s",&op);
   
           if(*op == 'n' || *op == 'N')menu(&num);
      
           }while(*op == 'y' || *op == 'Y'); 
           break;
    case 4:
          do{
            printf(
                   "\n\n\t*------------------*"
                   "\n\t|  REMOVE CONTACT  |"
                   "\n\t*------------------*\n"
                  );
           getChars(Nome);
           busca(raiz,Nome);
           printf("\n\n\tDo you want to continue (y)es or (n)o: ");
           scanf(" %s",&op);
           if(*op == 'n' || *op == 'N')menu(&num);
           else
           {
            excluir(&raiz,Nome);
            break;
           }
           }while(*op == 'y' || *op == 'Y');       
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
           printf("\n\n\tDo you want to continue (y)es or (n)o: ");
           scanf(" %s",&op);
           if(*op == 'y' || *op == 'Y')
           {
            alterar(&raiz,Nome);
            printf("\n\n\tDo you want to continue (y)es or (n)o: ");
            scanf(" %s",&op);
   
            if(*op == 'n' || *op == 'N')menu(&num);
            break;
           }
           else
           menu(&num);  
           }while(*op == 'y' || *op == 'Y');   
           break;
    case 6:
           printf("\n\tGood Bye.\n\n");
           break;   
  };
 }
 return 0;
}