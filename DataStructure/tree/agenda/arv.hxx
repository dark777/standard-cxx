#include <cstdio>
#include <cstdlib>
#include <cstring>

/*
  Autor: Marcos Augusto
  Email: marcosccp04@gmail.com
  Modified: Jean Zonta
*/

struct Arvore
{
 char Nome[100];
 int telefone;
 Arvore *esq;
 Arvore *dir;
};


 void inserir(Arvore**,char*);
 void ordem(Arvore*);
 void busca(Arvore*,char*);
 void alterar( Arvore**, char*);
 void excluir(Arvore**,char*);
 Arvore** menor_dir(Arvore*);
 Arvore** maior_esq(Arvore*);
 void ler_string(char*);
 void ler_telefone(int*);
 void maiuscula(char*);
 void menu(int*);
 void opcao(char*);