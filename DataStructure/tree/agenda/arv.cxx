#include "arv.hxx"

void inserir(Arvore **raiz ,char string[])
{
 int telefone;
 if(*raiz == NULL)
 {
  *raiz = new Arvore();
  strcpy((*raiz)->Nome,string);
  ler_telefone(&telefone);
  (*raiz)->telefone = telefone;
  (*raiz)->esq = NULL;
  (*raiz)->dir = NULL;              
 }
 else
 {
  if(strcasecmp((*raiz)->Nome,string)>0)
  inserir(&(*raiz)->esq,string);                     
  else
  if(strcasecmp((*raiz)->Nome,string)<0)
  inserir(&(*raiz)->dir,string);
  else
  if(strcmp((*raiz)->Nome,string)==0)
  printf("\n\tName already registered");
  getchar();
 }
}
                                                               
void ordem(Arvore *raiz)
{
 if(raiz!=NULL) 
 {
  ordem((raiz)->esq);
  printf(
         "\n\tName: %s"
         "\n\tPhone: %s",
         (raiz)->Nome,(raiz)->telefone
        );
      
  ordem((raiz)->dir);
 }
}

void busca(Arvore *raiz,char string[])
{
 if(raiz!=NULL)
 {
   if(strcasecmp((raiz)->Nome,string)>0)
    busca((raiz)->esq,string);
    else
     if(strcasecmp((raiz)->Nome,string)<0)
     busca((raiz)->dir,string);
    else
     if(strcmp((raiz)->Nome,string)==0)
     {
      printf(
             "\n\tRecord Found!!"
             "\n\tName: %s"
             "\n\tPhone: %s",
             (raiz)->Nome,(raiz)->telefone
            );
      getchar();
     }
  }  
 else
 {
  printf(
         "\n\t*------------------*"
         "\n\t| Name not found!! |"
         "\n\t*------------------*\n"
        );
  getchar();
 }
}

void alterar( Arvore **raiz, char *string)
{
  if((*raiz)!=NULL)
  {
   if(strcasecmp((*raiz)->Nome,string)>0)
    alterar(&(*raiz)->esq,string);
    else
   if(strcasecmp((*raiz)->Nome,string)<0)
    alterar(&(*raiz)->dir,string);
    else
   if(strcmp((*raiz)->Nome,string)==0)
   {
    int telefone;
    printf(
           "\n\tRecord Found!!"
           "\n\tName: %s",
           (*raiz)->Nome
          );
    
    printf("\n\tNew Phone: ");
    scanf("%d",&telefone);
    (*raiz)->telefone= telefone;
    printf(
           "\n\n\t*-----------------*"
           "\n\t| Changed data!! |"
           "\n\t*-----------------*\n"
          );
    getchar();
   }
  }
  else
  {
   printf(
          "\n\t*------------------*"
          "\n\t| Name not found!! |"
          "\n\t*------------------*\n"
         );
  }
}

void excluir(Arvore **raiz,char string[])
{
 Arvore **aux2, *aux3;     
 if(*raiz!=NULL)
 {
  if( strcasecmp((*raiz)->Nome , string)==0 )
  {
   if((*raiz)->esq == (*raiz)->dir)
   {
    free(*raiz);
    *raiz = NULL;
   }
   else
   {
    if((*raiz)->esq != NULL)
    {
     aux2 = maior_esq(*raiz);
     aux3 = *aux2;
     (*aux2) = (*aux2)->esq;
    }
    else
    {
     aux2 = menor_dir(*raiz);
     aux3 = *aux2;
     (*aux2) = (*aux2)->dir;
    }
    strcpy((*raiz)->Nome, aux3->Nome);
    free(aux3); aux3 = NULL;
   }
  }
  else
  {
   if(strcasecmp(string,(*raiz)->Nome)<0)
   {
    excluir(&(*raiz)->esq,string);
   }
   else
   {
    excluir(&(*raiz)->dir,string);
   }
  }
 }
 else
 {
  printf(
         "\n\n\t*----------------*"
         "\n\t| Name not found!! |"
         "\n\t*------------------*\n"
        );
  
  getchar();
 } 
}

Arvore** maior_esq(Arvore *raiz)
{
 Arvore **aux = &(raiz);
 if((*aux)->esq != NULL)
 {
   aux = &(*aux)->esq;
   while( (*aux)->dir != NULL )
   {
    aux = &(*aux)->dir;
   }
 }
 return aux;
}

Arvore** menor_dir(Arvore *raiz)
{
  Arvore **aux = &(raiz);
  if((*aux)->dir != NULL)
  {
    aux = &(*aux)->dir;
    while((*aux)->esq != NULL)
    {
      aux=&(*aux)->esq;
    }
  }
 return aux;
}

void menu(int *num)
{
 printf(
        "\n\n\tPHONEBOOK"   
        "\n\t|---------------------------|"
        "\n\t| 1 - Insert Contacts       |"
        "\n\t|---------------------------|"
        "\n\t| 2 - Print Contacts        |"
        "\n\t|---------------------------|"
        "\n\t| 3 - Search for Contacts   |"
        "\n\t|---------------------------|"
        "\n\t| 4 - Remove Contact        |"
        "\n\t|---------------------------|"
        "\n\t| 5 - Change Contact        |"
        "\n\t|---------------------------|"
        "\n\t| 6 - Exit                  |"
        "\n\t*---------------------------*"
        "\n\tDigite: "
       );
 
 scanf("%d",num);
 getchar();
}

void ler_telefone(int *telefone)
{
 printf("\n\tEnter phone number: ");
 scanf("%d",telefone);
}

void ler_string(char string[])
{
 printf("\n\tEnter the name: ");
 fflush(stdin);

 fgets(string,100,stdin);
 maiuscula(string);
}

void maiuscula(char string[])
{
 int i;
 for(i=0;i<strlen(string);i++)
 {
  if((string[i]>='a') && (string[i]<='z'))
  {
   string[i]-=32;
  } 
 } 
}

void opcao(char op[])
{
 int num; 
 printf("\n\tDo you want to continue (y)es or (n)o: ");
 scanf(" %s",&op);
 fflush(stdin);
 //if(*op == 'n' || *op == 'N')menu(&num);
}