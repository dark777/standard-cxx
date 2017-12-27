#include <iostream>
#include "carro.hxx"

int main(void)
{   
 int varIdMarca = 0;
 std::string varDescriMarca;

 std::cout<<"\n\tID DA MARCA: ";
 std::cin>>varIdMarca;
 
 do{
    
    std::cout<<"\n\tDESCRICAO DA MARCA: ";
    getline(std::cin,varDescriMarca);
    
   }while(varDescriMarca.length() == 0 || varDescriMarca == " ");
 
 Marca marc = *new Marca();
 marc.idMarca = varIdMarca;
 marc.descriMarca = varDescriMarca;

 
 int varIdModelo = 0;
 std::string varDescriModelo;

 std::cout<<"\n\tID DO MODELO: ";
 std::cin>>varIdModelo;
 
 do{
   
     std::cout<<"\n\tDESCRICAO DO MODELO: ";
     getline(std::cin,varDescriModelo);
    
   }while(varDescriModelo.length() == 0);          
 
 Modelo mod = *new Modelo();
 mod.idModelo = varIdModelo;
 mod.descriModelo = varDescriModelo;
 mod.marca = marc;
            
 int varIdMotor = 0;
 std::string varDescriMotor;
 std::string varPotenMotor;

 std::cout<<"\n\tID DO MOTOR: ";
 std::cin>>varIdMotor;
   
 do{
   
     std::cout<<"\n\tDESCRICAO DO MOTOR: ";
     getline(std::cin,varDescriMotor);
 
    }while(varDescriMotor.length() == 0 || varDescriMotor == " ");
 
 do{
   
     std::cout<<"\n\tPOTENCIA DO MOTOR: ";
     getline(std::cin,varPotenMotor);
   
   }while(varDescriMarca.length() == 0);
    
 Motor mot = *new Motor();
 mot.idMotor = varIdMotor;
 mot.descriMotor = varDescriMotor;
 mot.descriPotencia = varPotenMotor;
 
 int varIdCarro = 0;
 std::string varDescriCarro;

 std::cout<<"\n\tID DO CARRO: ";
 std::cin>>varIdCarro;

 do{
   
    std::cout<<"\n\tDESCRICAO DO CARRO: ";
    getline(std::cin,varDescriCarro);
   
   }while(varDescriCarro.length() == 0);
 
 Carro car = *new Carro();
 car.idCarro = varIdCarro;
 car.descriCarro = varDescriCarro;
 car.marca = marc;
 car.modelo = mod;
 car.motor = mot;
   
 std::cout<<"\n\t"<<car.imprimeDadosCarro()<<"\n\n";
 
 return 0;    
}
