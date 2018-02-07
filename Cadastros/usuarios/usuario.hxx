#include <cstring>
#include <iostream>

#if defined(linux) || defined(__linux) || \
defined(__linux__) || defined(__gnu_linux__)

 #include <stdio_ext.h>

#endif

struct Usuario
{
 char nome[101];
  char rg[10];
  char cpf[12];
  char email[50];
 char endereco[101];
};


void cadastrarUsuario();
void listarUsuarios();
void numeroDeUsuariosCadastrados();
int  contadorDeLinhasDeUmArquivo();
void insereUsuario(Usuario *);
void insereUsuarios(Usuario *, int);
int  numeroDeUsuarios();
void removerUltimoUsuario();
void sair();
void enter();
void cbuff();
void menu(); 
