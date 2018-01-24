#include "modelo.hxx"
#include "motor.hxx"

struct Carro
{
  Carro(){}
  ~Carro()
  {
   idCarro=0;
   if(descriCarro.length() != 0)descriCarro.clear();
  }
  int idCarro;
  std::string descriCarro;
  
  Marca marca;
  Modelo modelo;
  Motor motor;
  
 std::string imprimeDadosCarro()
 {
  std::string descricao = "\n\n\tID. DO CARRO: " + std::to_string(idCarro) +
                          "\n\n\tDESCRI. DO CARRO: " + descriCarro +
                          "\n\n\tID. DA MARCA: " + std::to_string(marca.idMarca) +
                          "\n\n\tDESCRI. DA MARCA: " + marca.descriMarca+
                          "\n\n\tID. DO MODELO: " + std::to_string(modelo.idModelo) +
                          "\n\n\tDESCRI. DO MODELO: " + modelo.descriModelo +
                          "\n\n\tID. DO MOTOR: " + std::to_string(motor.idMotor)+
                          "\n\n\tDESCRI. DO MOTOR: " + motor.descriMotor+
                          "\n\n\tDESCRI. DA POT. MOTOR: " + motor.descriPotencia;
  return descricao;
 }
 
};