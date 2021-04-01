#include <algorithm>
#include <iostream>
#include <time.h>
#include <windows.h> 

using namespace std;

int get_rand(int start, int end){
	return start + rand() % ((end - start ) + 1);
}

void output(int t[], int chg1, int chg2, int n){
	HANDLE  hConsole;
	hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	
	cout<<endl;
	
	for(int i = 0; i < n; i++){
		if(i == chg1 || i == chg2){
			SetConsoleTextAttribute(hConsole, 12);
			cout<<t[i];
			SetConsoleTextAttribute(hConsole, 7);
		}
		else if(chg1 == -1){
			SetConsoleTextAttribute(hConsole, 14);
			cout<<t[i];
			SetConsoleTextAttribute(hConsole, 7);
		}
		else cout<<t[i];
		cout<<"\t";
	}
	
	cout<<endl;
}

/*
void quick(int *t, int l, int r){
	if(l < r){
		int p = t[r];
		int ctr = l;

		for(int i = l; i < r; i++){
			if(t[i] < p){
				swap(t[i], t[ctr]);	
				ctr++;
			}
		}
		
		if(ctr != r) swap(t[r], t[ctr]);
		
		quick(t, l, ctr-1);
		quick(t, ctr+1, r);
	}
}
*/

void quick(int *t, int l, int r, bool out, int n){
	if(l < r){
		int p = t[r];
		int ctr = l;

		for(int i = l; i < r; i++){
			if(t[i] < p){
				if(ctr != i){
					swap(t[i], t[ctr]);
					output(t, i, ctr, n);
				} 
				
				ctr++;
			}
		}
		
		t[n]++;
		
		if(ctr != r){
			swap(t[r], t[ctr]);
			output(t, r, ctr, n);
		} 
		
		if(out) output(t, -1, -1, n);
		
		quick(t, l, ctr-1, out, n);
		quick(t, ctr+1, r, out, n);
	}
}

int main(){
	int n;
	bool out, gen;
	
	HANDLE  hConsole;
	hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
	
	cout<<"Rozmiar tablicy\n";
	cin>>n;
	cout<<"Czy wyswietlac tablice po kazdej iteracji? (1 = tak; 0 = nie)\n";
	cin>>out;
	cout<<"Czy generowac wartosci tablicy losowo? (1 = tak; 0 = nie)\n";
	cin>>gen;
	
	int t[n+1];
	t[n] = 0;
	
	srand (time(NULL));
	
	for(int i = 0; i < n; i++){
		if(gen) t[i] = get_rand(-1000, 1000);
		else{
			cout<<"Podaj "<<i<<" element: ";
			cin>>t[i];
			cout<<endl;
		}	
	}
	
	cout<<"(Zolty kolor oznacza tablice po iteracji)\nPrzed sortowaniem:"<<endl;
	for(int i = 0; i < n; i++){
		SetConsoleTextAttribute(hConsole, 12);
		cout<<t[i];
		SetConsoleTextAttribute(hConsole, 7);
		cout<<"\t";
	}
	cout<<endl<<endl;
	
	quick(t, 0, n-1, out, n);
	
	cout<<endl<<"Po sortowaniu"<<endl;
	for(int i = 0; i < n; i++){
		SetConsoleTextAttribute(hConsole, 10);
		cout<<t[i];
		SetConsoleTextAttribute(hConsole, 7);
		cout<<"\t";
	}
	
	cout<<"\nIlosc iteracji: "<<t[n];
	return 0;
}
