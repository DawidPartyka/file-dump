#include<iostream>
#include<math.h>
//Zadanie 2.2.16
// W Poleceniu nie by³o podane, ¿e nie mo¿na korzystaæ z bibliotek
// W rozk³adzie 0 nie bêdzie brane pod uwagê

using namespace std;

//Funkcja sprawdza czy do podanego parametru istnieje odpowiednie b^m
int pairs(long long int counter, long long int n, long long int m){
	long long int c = pow(counter, m), wynik = 0;
	
	for(long long int i = 1; i <= counter; i++){
		if(pow(i, m) + c == n){
			wynik = i;
			
			break;
		}
	}
	
	return wynik;
}


//Funkcja wypisuje wyniki
void output(long long int n, long long int m, int c){
	long long int counter;
	bool ctrl = false;
	
	counter = (int)pow(n, 1/(double)m);
	
	for(int i = 0; i < n; i++){
		long long int w = pairs(counter, n, m);
					
		if(w != 0){
			ctrl = true;
			
			switch(c){
				case 1:
					if(w != counter) cout<< counter << " + " << w <<endl;
					
					cout<< w << " + " << counter <<endl;
					break;
				
				case 2:
					cout<< w << " + " << counter <<endl;
					break;
			}
			
		}
		
		if(counter > 0) counter--;
		else break;
	}
	
	if(ctrl == false) cout<<"Brak rozwiazan";
}

int main(){
	long long int n, m;
	int c;
	
	cout<<"Podaj N i M (n > m)"<<endl;
	cin>>n>>m;
	
	cout<<"Wariant zadania: "<<endl;
	cout<<"1. a(z powtorzeniami)"<<endl;
	cout<<"2. b(bez powtorzen)"<<endl;
	cout<<"(Podaj cyfre)"<<endl;
	cin>>c;
	
	system("CLS");
	
	cout<<"Kolejne pary liczby ktore podniesione do potegi "<<m<<" dadza "<< n <<":"<<endl;

	output(n, m, c);
	return 0;
}
