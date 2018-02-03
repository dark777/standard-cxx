#include "usuario.hxx"
//https://github.com/codenome/Lista0-EstruturaDeDados/blob/master/Exericicio 7/
//https://pt.stackoverflow.com/questions/186716/como-excluir-a-%C3%BAltima-linha-de-um-arquivo-texto-na-linguagem-c
void enter()
{
#if defined(WIN32) || defined(_WIN32) || defined(__WIN32__)
 
  getch();

#elif defined(linux) || defined(__linux) || \
      defined(__linux__) || defined(__gnu_linux__)

  getchar();

#endif 

}

void cbuff()
{
 #if defined(WIN32) || defined(_WIN32) || defined(__WIN32__)
 
  fflush(stdin);
  
#elif defined(linux) || defined(__linux) || \
      defined(__linux__) || defined(__gnu_linux__)

  __fpurge(stdin);

#endif  
}

Usuario *_usuario;

void sair() 
{
 free(_usuario);
 std::cout<<"Good Bye ..!!!\n\n";
}

int contadorDeLinhasDeUmArquivo()
{
    char c;
    char letra = '\n';
    int vezes;
     
    FILE *arq = fopen("usuarios.txt","r");
    
        //Lendo o arquivo 1 por 1
    while(fread(&c, sizeof(char), 1, arq))
     {
      if(c == letra)vezes++;
     } 

    return vezes;

    fclose(arq);
}

void numeroDeUsuariosCadastrados() 
{
    std::cout << "Número de usuários cadastrados: " << contadorDeLinhasDeUmArquivo() << "\n";
}

int numeroDeUsuarios() 
{
    FILE *arquivo;
    int numeroDeLinhas = 0;

    arquivo = fopen("usuarios.txt", "r");

    if(arquivo == NULL) 
    {
     return 0;
    }

    char linha[500];
    while(fgets(linha, 500, arquivo))
    {
     ++numeroDeLinhas;
    }

    return numeroDeLinhas;
}

void listarUsuarios() 
{
    FILE *arquivo = fopen("usuarios.txt", "r");
    
    Usuario *usuario = (Usuario *) malloc(sizeof(Usuario));
    
    if(arquivo == NULL)
    {
     std::cout << "Erro na Abertura do arquivo\n";
     return;
    }

    if(numeroDeUsuarios() == 0)
    {
     std::cout << "Não Há usuários cadastrados\n";
     return;
    }

    char espaco = ' ';
    std::cout << "====================================================================================================================================================\n";
    printf("| Nome %-25c | RG %-13c | CPF %-13c | EMAIL %-30c | Endereço %-30c |\n", espaco, espaco, espaco, espaco, espaco);
    std::cout << "====================================================================================================================================================\n";

    char linha[500];
    while(fgets(linha, sizeof(linha), arquivo))
    {
     linha[strlen(linha)-1] = '\0';

     sscanf(
            linha, ";%[^;];%[^;];%[^;];%[^;];%[^;];",
            usuario->nome,
            usuario->rg,
            usuario->cpf,
            usuario->email,    
            usuario->endereco
           );
     
     printf(
            "|%-25s|%-14s|%-14s|%-35s|%-45s|\n",
            usuario->nome,
            usuario->rg,
            usuario->cpf,
            usuario->email,
            usuario->endereco
           );
    }

    std::cout << "\n\n";
    fclose(arquivo);
    free(usuario);
}

void cadastrarUsuario() 
{
    Usuario *usuario;
    int contador = numeroDeUsuarios();

    if(contador > 0) 
    usuario = (Usuario *) realloc(usuario, sizeof(Usuario));
    else
    usuario = (Usuario *) malloc(sizeof(Usuario));

    if(!usuario)
    {
     std::cout << "Exaustão de memória!\n";
     exit(1);
    }

    cbuff();
    std::cin.clear();
    std::cout << "Digite seu nome: ";
    std::cin.getline(usuario->nome, 100);

    cbuff();
    std::cin.clear();
    std::cout << "Digite seu RG: ";
    std::cin.getline(usuario->rg, 9);

    cbuff();
    std::cin.clear();
    std::cout << "Digite seu CPF: ";
    std::cin.getline(usuario->cpf, 11);
    
    cbuff();
    std::cin.clear();
    std::cout << "Digite seu EMAIL: ";
    std::cin.getline(usuario->email, 50);
    
    cbuff();
    std::cin.clear();
    std::cout << "Digite seu Endereço: ";
    std::cin.getline(usuario->endereco, 100);

    insereUsuario(usuario);
    free(usuario);
}

void insereUsuario(Usuario *pUsuario) 
{
    FILE * arquivo = fopen("usuarios.txt", "a+");

    if(arquivo == NULL)std::cout << "\n\tErro ao criar o arquivo\n\n";
    else
    fprintf(
            arquivo, ";%s;%s;%s;%s;%s;\n",
            pUsuario->nome,
            pUsuario->rg,
            pUsuario->cpf,
            pUsuario->email,    
            pUsuario->endereco
           );

    fclose(arquivo);
}

void removerUltimoUsuario() 
{
    FILE *arquivo = fopen("usuarios.txt", "r");
    
    if(arquivo == NULL)
    {
     std::cout << "Erro ao criar o arquivo\n";
    }
    
    Usuario *usuarios = (Usuario *) malloc(numeroDeUsuarios() * sizeof(Usuario));
    
    int i = 0;
    char linha[500];
    
    for(; fgets(linha, sizeof(linha), arquivo); i++)
    {
     linha[strlen(linha) - 1] = '\0';
     
     sscanf(
            linha, ";%[^;];%[^;];%[^;];%[^;];%[^;];",
            usuarios[i].nome,
            usuarios[i].rg,
            usuarios[i].cpf,
            usuarios[i].email,  
            usuarios[i].endereco            
           );
    }
    
    std::cout << "N: " << i << "\n";
    insereUsuarios(usuarios, i);
    fclose(arquivo);
    free(usuarios);
    std::cout << "Usuário removido com sucesso!\n";
}

void insereUsuarios(Usuario *pUsuario, int numUsuarios) 
{
    FILE * arquivo = fopen("usuarios.txt", "w");

    if(arquivo == NULL)
    {
     std::cout << "Erro ao criar o arquivo";
    }

    std::cout << "N: " << numUsuarios << "\n";

    for(int i = 0; i < (numUsuarios-1); i++)
    {
     fprintf(
             arquivo, ";%s;%s;%s;%s;%s;\n",
             pUsuario->nome,
             pUsuario->rg,
             pUsuario->cpf,
             pUsuario->email,    
             pUsuario->endereco
            );
    }

    fclose(arquivo);
}


void menu() 
{
    int escolha = 0;

    std::cout << "1. Cadastrar usuário\n"
                 "2. Exibir usuario(s) cadastrado(s)\n"
                 "3. Exibir quantidade de usuario(s) cadastrado(s)\n"
                 "4. Remover o ultimo usuário cadastrado\n"
                 "5. Sair\n"
                 "Digite uma opção válida: ";
     
    std::cin.clear();
    cbuff();
    std::cin >> escolha;

    switch (escolha) 
    {
     case 1:
      cadastrarUsuario();
     break;
     
     case 2:
      listarUsuarios();
     break;
     
     case 3:
      numeroDeUsuariosCadastrados();
     break;
     
     case 4:
      removerUltimoUsuario();
     break;
     
     case 5:
      sair();
     return;
     
     default:
      std::cout << "Opção inválida! Tente novamente...\n";
    }

    cbuff();
    std::cout << "Pressione ENTER para prosseguir...\n";
    enter();
    menu();
} 
