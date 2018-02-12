/* ListarDiretorios.cpp:
   - Listar diretorios
   - Criar recursão para acessar subpastas
   - Procurar arquivos via extensão
   - Adicionar arquivos em uma coleção
   - Estruturar o programa em classes
*/

#include <iostream>
#include <dirent.h>

#define IS_FILE 32768
#define IS_DIR 16384

const char *DIRECTORY = "/home/";

struct dirent *directory = 0;
DIR *_DIR;

using namespace std;


//prototipos
int countFiles(const char *);
inline bool existsFiles(const char *);
void listDir(const char *);
inline bool dirExist(const char *);

int main()
{
 //cout << countFiles(DIRECTORY) << endl;
 cout << dirExist(DIRECTORY) << endl;
 listDir(DIRECTORY);
 return 0;
}

inline bool dirExist(const char *dir)
{
 return ((::_DIR = opendir(dir)) != NULL);
}

void listDir(const char *dir)
{
 ::_DIR = opendir(dir);
 
 while ((directory = readdir(_DIR)) != NULL)
  {
   if(directory->d_type == IS_DIR)
   cout << DIRECTORY << directory->d_name << endl;
  };
 
 directory = 0;
 closedir(::_DIR);
}

inline bool existsFiles(const char *dir)
{
 ::_DIR = opendir(dir);
 
 while ((directory = readdir(_DIR)) != NULL)// exists file
  {
   if(directory->d_type == IS_FILE)
    {
     directory = 0;
     closedir(::_DIR);
     return 1;
    }
  };

  directory = 0;
  closedir(::_DIR);
  return 0;
}

int countFiles(const char *dir)
{
 int count = 0;
 ::_DIR = opendir(dir);
 
 while ((directory = readdir(_DIR)) != NULL)
 {
  if (directory->d_type == IS_DIR)
  count++;
 };

 directory = 0;
 closedir(::_DIR);
 return count;
}