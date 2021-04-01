#include <iostream>
#include <math.h>
#include <conio.h>
#include <stdlib.h>
#include <time.h> 
#include <stdio.h> 

using namespace std;

//Ocenia wartoœæ komórki. (Tu wpisaæ funkcjê do optymalizacji) najwa¿niejsza funkcja w programie. 
int fitness(int chr[]){
	return (chr[0] + chr[1]) - (chr[2] + chr[3]) + (chr[4] - chr[5]) - (chr[6] + chr[7]);
}



//Krzy¿owanie single point
int single_point(int chr[], int chr_sub[], int i, int z, int gens){
	float frac = gens / 2;
	
	if(frac != (int)frac) frac = (int) frac + 1;
	
	switch (i){
		case 0:
			if(z < frac) return chr[z];
			else return chr_sub[z];
			break;
		
		case 1:
			if(z < frac) return chr_sub[z];
			else return chr[z];
			break;
	}
}


//Krzy¿owanie two point
int two_point(int chr[], int chr_sub[], int i, int z, int gens){
	float frac = gens / 4;
	
	if(frac != (int)frac) frac = (int) frac + 1;
	
	int end = gens - frac - 1;
	
	switch (i){
		case 0:
			if(z < frac || z >= end) return chr[z];
			else return chr_sub[z];
			break;
		
		case 1:
			if(z < frac || z >= end) return chr_sub[z];
			else return chr[z];
			break;
	}
}


//Losuje liczby do losowoœci w krzy¿owaniach i mutacjach
int get_rand(int start, int end){
	return start + rand() % ((end - start ) + 1);
}


//sortuje wed³ug "si³y" chromosomu
int fit(int maxs[], int z, int fits[], int pop){
	int max = -1000;
	int pos;
	
	for(int i = 0; i < pop; i++){
		bool chk = true;
		
		for(int z = 0; z < pop; z++){
			if(i == fits[z]) chk = false;
		}
		
		if(chk){	
			if(maxs[i] > max){
				pos = i;
				max = maxs[i];
			} 			
		}
	}
	
	return pos;
}



void start(int vars[]){
	int pop = vars[0], 
	mut = vars[1], 
	min_g = vars[2], 
	max_g = vars[3], 
	generations = vars[4], 
	max_fit = vars[5], 
	gens = vars[6];
	
	int chr[pop][gens];
	int best_fit[2][gens];
	int fits[pop];
	
	best_fit[0][0] = 0;
	
	for(int i = 1; i < gens; i++){
		best_fit[1][i] = 0;
	}
			
	cout << endl;
	
	cout << "Max val gen: " << max_g << " Min val gen: " << min_g << " Maksimum globalne: " << max_fit << endl;
	cout << endl;
	
	///////////////Pêtla generacji///////////////////////
	for(int g = 0; g < generations; g++){

		//Wype³nia ka¿dy chromosom losowymi genami
		if(g == 0){
			for(int i = 0; i < pop; i++){
				for(int z = 0; z < gens; z++){		
					chr[i][z] = get_rand(min_g, max_g);
				}
			}
			
			cout << "START: " << endl;
			for(int i = 0; i < pop; i++){
				cout << "chromosom " << i << "{ ";
		
				for(int z = 0; z < gens; z++){
					cout << chr[i][z] <<" , ";	
				}

				cout << "fitness: " << fitness(chr[i]);
				cout << " }" << endl; 
			}
			
			cout<< endl;
		}
		///////////////////////////////////////////
		
		cout << "GENERACJA " << g << endl;
				
		//////Sortuje chromosomy wed³ug ich "si³y"/////
		int maxs[pop];
		
		for(int i = 0; i < pop; i++){
			maxs[i] = fitness(chr[i]);
			fits[i] = fit(maxs, i, fits, pop);
		}
		
		/////////////////////////////////////
		
		if(fitness(chr[fits[0]]) > best_fit[0][0]){
			best_fit[0][0] = fitness(chr[fits[0]]);
			
			for(int i = 0; i < gens; i++){
				best_fit[1][i] = chr[fits[0]][i];
			}
		}
		

		if(best_fit[0][0] == max_fit) break;

		
		/////////////////////////Krzy¿owanie genów////////////////////////
		for(int i = 0; i < 4; i++){
			for(int z = 0; z < gens; z++){	
				int pos = pop - i - 1;
				
				/////Krzy¿owanie single point genów dominuj¹cych chromosomów/////
				if(i < 2) chr[fits[pos]][z] = single_point(chr[fits[0]], chr[fits[1]], i, z, gens); 
				/////////////////////////////////////////////////////////////////

				/////Krzy¿owanie two point genów dominuj¹cych chromosomów////////
				else if(i > 1)chr[fits[pos]][z] = two_point(chr[fits[1]], chr[fits[2]], i - 2, z, gens);
				/////////////////////////////////////////////////////////////////
			}
		}
		
		
		/////////////KRZY¯OWANIE UNIFORM dominuj¹cych chromosomów////////////////
		int rd_uni;
		int tmpr;
		int swap;
		
		rd_uni = get_rand(0, gens - 1);
		
		for(int i = 0; i < 4; i+=2){
			for(int z = 0; z < rd_uni; z++){
				tmpr = get_rand(0, gens - 1);
				swap = chr[fits[i]][tmpr];
				
				chr[fits[i]][tmpr] = chr[fits[i+1]][tmpr];
				chr[fits[i+1]][tmpr] = swap;
			}	
		}
			
		////////////KRZY¯OWANIE UNIFORM reszty chromosomów////////////////
		for(int i = 0; i < pop - 6; i += 2){
			tmpr = 0;		
			rd_uni = get_rand(0, gens - 1);
			
			for(int z = 0; z < rd_uni; z++){
				tmpr = get_rand(0, gens - 1);
				
				swap = chr[fits[i]][tmpr];
				
				chr[fits[i]][tmpr] = chr[fits[i + 1]][tmpr];
				chr[fits[i + 1]][tmpr] = swap;
			}

		}
		/////////////////////////////////////////////////////////////////
		
		///////////MUTACJE/////////////
		for(int i = 0; i < pop; i++){
			if(get_rand(0, 100) < mut){
				int tmp = get_rand(0, gens - 1);
				
				for(int y = 0; y < tmp; y++){
					int tmpr = get_rand(0, gens - 1);
				
					chr[fits[i]][tmpr] = get_rand(min_g, max_g);
				}
			}
		}
		///////////////////////////////
		/////////////////////////////////////////////////////////////////
		
		
		if(fitness(chr[fits[0]]) > best_fit[0][0]){
			best_fit[0][0] = fitness(chr[fits[0]]);
			
			for(int i = 0; i < gens; i++){
				best_fit[1][i] = chr[fits[0]][i];
			}
		}
		
		cout << "FITTEST:" << endl;

		for(int i = 0; i < 3; i++){
			cout << i << ". " << " fitness: " << fitness(chr[fits[i]]) << endl; 
		}
		cout << endl;
		
		
		if(best_fit[0][0] == max_fit) break;

		
		for(int i = 0; i < 3; i++){
		cout << "chromosom " << i << "{ ";
		
			for(int z = 0; z < gens; z++){
				cout << chr[fits[i]][z] <<" , ";	
			}

			cout << "fitness: " << fitness(chr[fits[i]]);
			cout << " }" << endl; 
		}
		cout<<endl;
		cout<<endl;
	}
	////////////////////////////////////////////////////
	
	for(int i = 0; i < 3; i++){
		cout << "chromosom " << i << "{ ";
		
		for(int z = 0; z < gens; z++){
			cout << chr[fits[i]][z] <<" , ";	
		}

		cout << "fitness: " << fitness(chr[fits[i]]);
		cout << " }" << endl; 
	}
	cout<<endl;
		
	cout<<endl;
	cout<<"Najlepszy wynik: "<<endl;
	cout<<"Fitness: "<< fitness(best_fit[1])<<endl;
	cout<<"Chromosom { "<< endl;
			
	for(int i = 0; i < gens; i++){
		cout<<best_fit[1][i]<< endl;
	}
			
	cout<<" }";
	
	getch();
}



int main(){
	int vars[7];// pop, mut, max_fit, gens = 8, min_g, max_g, generations;
	
	cout << "Jaka bedzie populacja startowa?" << endl;
	cin >> vars[0];
	
	cout << "Jak duza bedzie szansa mutacji w procentach? (int)"<<endl;
	cin >> vars[1];
	
	cout << "Podaj przedzial wartosci dla genow (int)" << endl;
	cin >> vars[2] >> vars[3];
	
	cout << "Ile maksymalnie ma powstac generacji?" << endl;
	cin >> vars[4];
	
	system("cls");
	
		
	vars[5] = (2 * vars[3]) - (2 * vars[2]) + (vars[3] - vars[2]) - (2 * vars[2]); // <-- max_fitness !! MAKSIMUM GLOBALNE DLA USTALONEJ TERAZ FUNKCJI FITNESS PRZY 8 GENACH NA CHROMOSOM !!
	//^ potrzebne aby pêtla start zatrzyma³a siê w przypadku znalezienia optymalnego rozwi¹zania 
	
	vars[6] = 8; // liczba genów
	
	srand (time(NULL));
	
	start(vars);
	
	return 0;
}
