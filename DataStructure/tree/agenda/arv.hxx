#include <cstdio>
#include <cstring>
#include <cstdlib>

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
void getChars(char*);
void getPhones(int*);
void maiuscula(char*);
void menu(int*);
void opcao(char*);
