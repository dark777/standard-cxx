#include <cstring>
#include <iostream>

#if defined(WIN32) || defined(_WIN32) || defined(__WIN32__)
 
 #include <conio.h>
 #include <windows.h>

#elif defined(linux) || defined(__linux) || \
      defined(__linux__) || defined(__gnu_linux__)

 #include <stdio_ext.h>

#endif

struct Usuario
{
 char nome[101];
  char rg[10];
  char cpf[12];
 char endereco[101];
};

void sair();
void menu();
void enter();
void listarUsuarios();
void cadastrarUsuario();
void removerUltimoUsuario();
void numeroDeUsuariosCadastrados();
void insereUsuario(Usuario *pUsuario);
int numeroDeUsuarios();
int contadorDeLinhasDeUmArquivo();