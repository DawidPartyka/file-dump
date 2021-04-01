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
	
	int end = gens - frac - 1;
	if(frac != (int)frac) frac = (int) frac + 1;
	
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
int get_rand(int start, int gens){
	//gens++;
	//return rand() % gens + start;
	return start + rand() % ((gens - start ) + 1);
}


//Generuje losowe "geny"
int start_population(int min_g, int max_g){
	/*max_g++;
	
	return rand() % max_g + min_g;*/
	return min_g + rand() % ((max_g - min_g ) + 1);
}


//sortuje wed³ug "si³y" chromosomu
int fit(int maxs[], int z, int fits[], int pop){
	int max = -10000;
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
					chr[i][z] = start_population(min_g, max_g);
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
		int fits[pop];
		int maxs[pop];
		int tmp[pop][gens];
		
		
		for(int i = 0; i < pop; i++){
			maxs[i] = fitness(chr[i]);
			fits[i] = -100;
		}
	
		for(int i = 0; i < pop; i++){		
			fits[i] = fit(maxs, i, fits, pop);
		}
		
		for(int i = 0; i < pop; i++){
			for(int z = 0; z < gens; z++){
				tmp[i][z] = chr[fits[i]][z];
			}
		}
		
		for(int i = 0; i < pop; i++){
			for(int z = 0; z < gens; z++){
				chr[i][z] = tmp[i][z];
			}
		}
		/////////////////////////////////////
		
		if(fitness(chr[0]) > best_fit[0][0]){
			for(int i = 0; i < gens; i++){
				best_fit[1][i] = chr[0][i];
			}
		}
		

		if(fitness(chr[0]) == max_fit) break;

		
		/////////////////////////Krzy¿owanie genów////////////////////////
		for(int i = 0; i < 5; i++){
			for(int z = 0; z < gens; z++){	
				
				/////Krzy¿owanie single point genów dominuj¹cych chromosomów/////
				if(i < 2) chr[pop - i - 1][z] = single_point(chr[fits[0]], chr[fits[1]], i, z, gens); 
				/////////////////////////////////////////////////////////////////
				
				else if(i == 3) continue;
				
				/////Krzy¿owanie two point genów dominuj¹cych chromosomów////////
				else if(i >= 3)chr[pop - i][z] = two_point(chr[fits[1]], chr[fits[2]], i - 3, z, gens);
				/////////////////////////////////////////////////////////////////
			}
			
			//////////////////////Mutacja//////////////////////////
			if(get_rand(0, 100) <= mut){
				int tmp = get_rand(0, gens);
				
				for(int y = 0; y < tmp; y++){
					int tmpr = get_rand(0, gens - 1);
				
					chr[pop - i - 1][tmpr] = get_rand(min_g, max_g);
				}
			} 
			///////////////////////////////////////////////////////
		}
		
		
		/////////////KRZY¯OWANIE UNIFORM dominuj¹cych chromosomów////////////////
		int rd_uni_ex;
		int tmpr_ex = 0;
		int swap_ex;
			
		rd_uni_ex = get_rand(0, gens - 1);
			
		for(int z = 0; z < rd_uni_ex; z++){
			tmpr_ex = get_rand(0, gens);
				
			swap_ex = chr[pop - 5][tmpr_ex];
				
			chr[pop - 5][tmpr_ex] = chr[pop - 6][tmpr_ex];
			chr[pop - 6][tmpr_ex] = swap_ex;
		}
		
		/////////////////Mutacja dominuj¹cych chromosomów//////////////////////////
			for(int q = 0; q < 2; q++){
				if(get_rand(0, 100) <= mut){
					int tmp = get_rand(0, gens - 1);
				
					for(int z = 0; z < tmp; z++){
						int tmpr = get_rand(0, gens);
				
						chr[pop - 6 + q][tmpr] = get_rand(min_g, max_g);;
					}
				} 
			}
		//////////////////////////////////////////////////
			
		////////////KRZY¯OWANIE UNIFORM reszty chromosomów////////////////
		for(int i = 0; i < pop - 6; i += 2){
			int rd_uni;
			int tmpr = 0;
			int swap;
			
			rd_uni = get_rand(0, gens - 1);
			
			for(int z = 0; z < rd_uni; z++){
				tmpr = get_rand(0, gens);
				
				swap = chr[i][tmpr];
				
				chr[i][tmpr] = chr[i + 1][tmpr];
				chr[i + 1][tmpr] = swap;
			}
			
			/////////////////Mutacja//////////////////////////
			for(int q = 0; q < 2; q++){
				if(get_rand(0, 100) <= mut){
					int tmp = get_rand(0, gens - 1);
				
					for(int z = 0; z < tmp; z++){
						tmpr = get_rand(0, gens);
				
						chr[i + q][tmpr] = get_rand(min_g, max_g);
					}
				} 
			}
			//////////////////////////////////////////////////
			
		}
		/////////////////////////////////////////////////////////////////
		/////////////////////////////////////////////////////////////////
		
		
		if(fitness(chr[0]) > best_fit[0][0]){
			for(int i = 0; i < gens; i++){
				best_fit[1][i] = chr[0][i];
			}
		}
		
		cout << "FITTEST:" << endl;

		for(int i = 0; i < 3; i++){
			cout << i << ". " << " fitness: " << fitness(chr[i]) << endl; 
		}
		cout << endl;
		
		
		if(fitness(chr[0]) == max_fit) break;

		
		for(int i = 0; i < 3; i++){
		cout << "chromosom " << i << "{ ";
		
			for(int z = 0; z < gens; z++){
				cout << chr[i][z] <<" , ";	
			}

			cout << "fitness: " << fitness(chr[i]);
			cout << " }" << endl; 
		}
		cout<<endl;
		cout<<endl;
	}
	////////////////////////////////////////////////////
	
	for(int i = 0; i < 3; i++){
		cout << "chromosom " << i << "{ ";
		
		for(int z = 0; z < gens; z++){
			cout << chr[i][z] <<" , ";	
		}

		cout << "fitness: " << fitness(chr[i]);
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
