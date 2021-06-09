//Bibliotecas necessárias

#include <stdio.h>
#include <stdlib.h>
#include <locale.h>
#include <windows.h> // Para usar o Sleep() comando que definirá o tempo de repetição entre os loops
 
int main(void){
    // comando de regionalização "Apenas para adicionar a acentuação"
    setlocale(LC_ALL, "Portuguese");
    // título do programa
    SetConsoleTitle("Cronometro");
    
    //variáveis
    int Segundos=0; //segundos
    int Minutos=0; //minutos
    int Horas=0; //horas
    
    int i=0; // Enquanto a variável i for 0 o loop se repetirá, ou seja, infinitamente. 
        
    do {
        system("cls"); // limpar a tela a cada loop para que fique apenas o cronometro na tela
        
        printf("\tCronômetro\n\n");
        
        printf("\n\t%dh:%dm:%ds\n\n", Horas,Minutos,Segundos); // \n equivale ao "Enter" do teclado e pula a linha, \t para simular o "tab" do teclado e dar maior espaçamento, apenas estica rs
        
        if(Segundos==60){
            Segundos=0;
            Minutos++;
        }
        
        if(Minutos==60){
            Minutos=0;
            Horas++;
        }
        
        if(Horas==24){
            Horas=0;
        }
        
        Sleep(1000); // o sistema por padrão executaria o loop 1 vez a cada ms, Função que marca o tempo até que o laço de repetição inicie novamente, "1000" para equivaler a 1s
        Segundos++;
        
    }while (i==0); // condição para que o loop seja infinito, enquanto for zero será infinito.
 
    system("pause");
    return 0;
}
