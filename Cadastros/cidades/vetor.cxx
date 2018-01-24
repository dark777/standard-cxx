#include <cstdio>
#include <cstdlib>
#include <stdio_ext.h>

#define TAM 3 //tamanho do vetor Reg[]

struct Reg 
{
 int codigo;
  char nomeCidade[30];
  char estado[5];
  int numeroVeiculos;
  int numeroAcidentes;
 int status;//status do registro: 1==ATIVO, 0==INATIVO
};
        
int last_pos=0;
Reg r[TAM];
     
void GetInfo(void)
{
 //Armazena dados no vetor Reg[]
 printf("\n\tCidade: ");
 fgets(r[last_pos].nomeCidade,30,stdin);
 __fpurge(stdin); //limpa entrada de dados
 
 printf("\n\tEstado: ");
 fgets(r[last_pos].estado,5,stdin);
 __fpurge(stdin); //limpa entrada de dados
 
 printf("\n\tCodigo: ");
 scanf("%i", &r[last_pos].codigo);
 __fpurge(stdin); //limpa entrada de dados
 
 printf("\n\tNumero Veiculos: ");
 scanf("%i", &r[last_pos].numeroVeiculos);
 __fpurge(stdin); //limpa entrada de dados
 
 printf("\n\tNumero Acidentes: ");
 scanf("%i", &r[last_pos].numeroAcidentes);
 __fpurge(stdin);
 
 r[last_pos].status=1;
 last_pos++;
}


void numRegistros()
{
 printf("\n\tNumero de Registros.: "); 
 for(register int i=0; i<=last_pos; i++) 
   if(r[i].status == 0)
     printf("%i\n\n",i++);
}

void ShowInfo(void)
{
 for(register int i=0; i<=last_pos; i++) 
  {
   if(r[i].status == 1)
    {
     printf(
            "\n\tNumero Registro.: %i"             
            "\n\tCidade..........: %s"
            "\tEstado..........: %s"
            "\tCodigo..........: %i"
            "\n\tNumero Veiculos.: %i"
            "\n\tNumero Acidentes: %i\n\n",
            i, r[i].nomeCidade, r[i].estado, r[i].codigo, r[i].numeroVeiculos, r[i].numeroAcidentes
           );
    }
  }
}

//altera o status do regisro
void DelInfo(int *n) 
{
 r[*n].status=0; 
}//fim DelInfo()
     
int main()
{
  int opcao, del;
    
  //Armazena dados no vetor Reg[]
  while(opcao != 5) 
   { 
    printf(
           "\n\t1. Adicionar"
           "\n\t2. Total Registros"     
           "\n\t3. Remover"
           "\n\t4. Listar" 
           "\n\t5. Sair"   
           "\n\tOpcao.: "
          );
    
    scanf("%i", &opcao);
    __fpurge(stdin);    

    switch(opcao) 
     {
      case 1:
       GetInfo();
       break;
      
      case 2:
       numRegistros();
      break;
      
      case 3:
       printf("\n\tQual Registro quer excluir? ");
       scanf("%i", &del);
       __fpurge(stdin);
       DelInfo(&del);
       break;
      
      case 4:
       ShowInfo();
       break;
      
      case 5:
       printf("\n\tGood Bye!!!\n\n");
       return 0;
       break;
      
      default:
       printf("\n\tOpcao Invalida!!!\n\n");
       break;
     }
   }
 return 0;
}