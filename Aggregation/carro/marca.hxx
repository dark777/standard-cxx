#include <iostream>

struct Marca
{
 Marca(){}
  ~Marca()
  {
   idMarca=0;
   if(descriMarca.length() != 0)descriMarca.clear();
  }
  int idMarca;
 std::string descriMarca;
};

