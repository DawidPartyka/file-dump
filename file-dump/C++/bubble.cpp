#include <iostream>

using namespace std;

int main(){
	int n, counter = 0;
	bool f;
	
	cout<<"Podaj ilosc elementow tablicy\n";
	
	cin>>n;
	
	cout<<"-----INPUT-----\n";
	
	int t[n];
	
	for(int i = 0; i < n; i++){
		cin>>t[i];
	}
	
	for(int j = 0; j < n; j++){
		f = true;
		counter++;
		for(int i = 0; i < n - 1; i++){
			if(t[i] > t[i+1]){
				swap(t[i], t[i+1]);
				f = false;
			} 
		}
		if(f) break;
	}
	
	cout<<"----OUTPUT----\n";
	
	for(int i = 0; i < n; i++){
		cout<<t[i]<<endl;
	}
	
	cout<<endl<<endl<<"Ilosc iteracji: "<<counter;
	
	return 0;
}
