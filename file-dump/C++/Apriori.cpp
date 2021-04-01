#include <vector>
#include <iostream>
#include <time.h> 
#include <conio.h>
#include <stdlib.h>
#include <algorithm>
using namespace std;

int get_rand(int start, int end){
	return start + rand() % ((end - start ) + 1);
}

class pojProd{
	public:
		vector < string > nazwa;
		vector < float > prodProb;
		vector < int > wyst;
		int wszystProd;
		float max;
		
		pojProd();
		void fill(pojProd& u, int p);
		void probFill(pojProd& u);
		void unProb(pojProd& u, float uf);
};

class koszyk{
	public:
		vector < int > prod;
		void fill(koszyk& u, int pb, int pe, pojProd& z);
		void out(koszyk u, pojProd& z, bool w, int q);
		void emptyProd(koszyk& u, pojProd x);
		int retSize(koszyk u);
};

class paryProd{
	public:
		vector < int > prodA;
		vector < int > prodB;
		vector < int > wyst;
		vector < float > paraProb;
		float max;
		
		paryProd();
		void countProb(paryProd& u, pojProd z);
		void unProb(paryProd& u, float p);
		void countWyst(paryProd& u, koszyk x[], int n);
		void par(paryProd& u, pojProd z, koszyk x[], int n);
		bool checkRep(paryProd u, int a, int b);
};

class result{
	public:
		vector < int > id;
		vector < int > prodFin;
		vector < int > wyst;
		vector < float > prob;
		float max;
		
		result();
		void par(result& u, paryProd z, koszyk x[], int n);
		bool checkRep(result u, int a, int b);
		void countWyst(result& u, paryProd z, koszyk x[], int n);
		void unProb(result& u, float p);
		void countProb(result& u, pojProd z);
		void exReps(result&u, paryProd z);
		void output(result u, paryProd z, pojProd x);
};

pojProd::pojProd(){
	wszystProd = 0;
	max = 0;
}

paryProd::paryProd(){
	max = 0;
}

result::result(){
	max = 0;
}

float checkProb(int a, int b){
	float x = (float)a / (float)b;
	if(x > 1) x = (float)b / (float)a;
	return x;
}

int main(){
	srand (time(NULL));
	
	pojProd u;
	result res;
	paryProd pary;

	int n, p, pb, pe;
	float uf = 0.2, x;
	bool out;
	
	cout<<"Ile bedzie koszykow?\n";
	cin>>n;
	
	cout<<"Ile bedzie produktow?\n";
	cin>>p;
	
	cout<<"Podaj minimalna i maksymalna ilosc przedmiotow w koszyku\n";
	cin>>pb>>pe;
	
	cout<<"Wyswietlic koszyki z zawartoscia? (boolean)\n";
	cin>>out;
	
	cout<<"Podaj prog ufnosci (float) lub wpisz 0 dla domyslnych 20%\n";
	cin>>x;
	
	if(x != 0) uf = x;
	
	u.fill(u, p);
	koszyk kosz[n];
	
	for(int i = 0; i < n; i++){
		kosz[i].fill(kosz[i], pb, pe, u);
	}
	
	for (int i = 0; i < n; i++){
		kosz[i].out(kosz[i], u, out, i);
	}		
	
	u.probFill(u);
	u.unProb(u, uf);

	for (int i = 0; i < n; i++){
		kosz[i].emptyProd(kosz[i], u);	
	}

	pary.par(pary, u, kosz, n);
	pary.countWyst(pary, kosz, n);
	pary.countProb(pary, u);
	pary.unProb(pary, uf);

	res.par(res, pary, kosz, n);
	res.countWyst(res, pary, kosz, n);
	res.countProb(res, u);
	res.unProb(res, uf);
	res.exReps(res, pary);
	res.output(res, pary, u);
	return 0;
}

////////////Funkcje result////////////
void result::par(result& u, paryProd z, koszyk x[], int n){
	int a, b, tmp;
	bool c, d;

	for(int i = 0; i < z.wyst.size(); i++){
		if(z.paraProb[i] > 0){
			a = z.prodA[i]; 
			b = z.prodB[i];

			for(int j = 0; j < n; j++){
				if(x[j].retSize(x[j]) >= 3){
					c = false;
					d = false;

					for(int q = 0; q < x[j].prod.size(); q++){
						if(x[j].prod[q] == a) c = true;
						if(x[j].prod[q] == b) d = true;
						if(c && d) break;
					}

					if(c && d){
						for(int q = 0; q < x[j].prod.size(); q++){
							if(x[j].prod[q] != a || x[j].prod[q] != b){
								if(!u.checkRep(u, i, q)){
									u.id.push_back(i);
									u.prodFin.push_back(q);
									u.wyst.push_back(0);
								}
							}
						}
					}	
				}
			}
		}
	}
}

bool result::checkRep(result u, int a, int b){
	for(int i = 0; i < u.id.size(); i++){
		if(u.id[i] == a){
			if(u.prodFin[i] == b) return true;
		}
	}

	return false;
}

void result::countWyst(result& u, paryProd z, koszyk x[], int n){
	int a, b, c, d = 0, e = 0, f = 0;

	for(int i = 0; i < u.id.size(); i++){
		a = z.prodA[i];
		b = z.prodB[i];
		c = u.prodFin[i];

		for(int j = 0; j < n; j++){
			for(int q = 0; q < x[j].retSize(x[j]); q++){
				if(x[j].prod[q] == a) d++;
				if(x[j].prod[q] == b) e++;
				if(x[j].prod[q] == c) f++;
			}

			if(d > 0 && e > 0 && f > 0){
				if(e > d) swap(d, e);
				d = (int)(d/e);

				if(f > d) swap(d, f);
				u.wyst[i] += (int)(d/f);
			}
			d = 0; e = 0; f = 0;
		}
	}
}

void result::countProb(result& u, pojProd z){
	float x;
	for(int i = 0; i < u.id.size(); i++){
		x = checkProb(u.wyst[i], z.wyst[i]);
		if(x > 0){
			if(x > u.max) u.max = x;
		} 
		
		u.prob.push_back(x);
	}
}

void result::unProb(result& u, float p){
	for(int i = 0; i < u.id.size(); i++)
		if(u.prob[i] <= p) u.prob[i] = -1;
}

void result::exReps(result&u, paryProd z){
	int a, b, c;
	bool d, e, f;

	for(int i = 0; i < u.id.size(); i++){
		if(u.prob[i] > 0){
			a = u.prodFin[i];
			b = z.prodA[u.id[i]];
			c = z.prodB[u.id[i]];
			d = false; e = false; f = false;

			for(int j = 0; j < u.id.size(); j++){
				if(j == i) continue;
				else if(u.prob[j] < 0) continue;

				if(a == u.prodFin[j]) d = true;
				if(!d) break;

				if(b == z.prodB[u.id[j]]) e = true;
				if(c == z.prodA[u.id[j]]) f = true;
				if(d && e && f){
					u.prob[j] = -1;
					break;
				}
			}
		}
	}
}

void result::output(result u, paryProd z, pojProd x){
	int tmp, t1, t2;
	bool f = false;
	
	cout<<endl;
	
	for(int i = 0; i < u.id.size(); i++){
		if(u.prob[i] > 0){
			if(u.prodFin[i] != z.prodA[i] || u.prodFin[i] != z.prodB[i]){
				f = true;
				tmp = u.id[i];
				t1 = z.prodA[tmp]; t2 = z.prodB[tmp];
				cout<<"\n( "<<x.nazwa[t1]<<" "<<x.nazwa[t2]<<" ) => ";
				cout<<x.nazwa[u.prodFin[i]]<<" Prawdopodobienstwo: ";
				cout<<u.prob[i]*100<<"%";
			}
			
		}
	}
	
	if(!f) cout<<"Brak wynikow.";
}

////////////Funkcje par////////////
void paryProd::par(paryProd& u, pojProd z, koszyk x[], int n){
	for(int i = 0; i < z.wyst.size(); i++){
		if(z.prodProb[i] > 0){

			for(int j = 0; j < n; j++){
				if(x[j].retSize(x[j]) >= 2){

					for(int q = 0; q < x[j].retSize(x[j]); q++){	
						if(x[j].prod[q] != i){
							
							if(!u.checkRep(u, i, x[j].prod[q])){
								u.prodA.push_back(i);
								u.prodB.push_back(x[j].prod[q]);
								u.wyst.push_back(0);
							}
						}
					}	
				}
			}
		}
	}
}

void paryProd::countWyst(paryProd& u, koszyk x[], int n){
	int a, b, c = 0, d = 0;

	for(int i = 0; i < u.prodA.size(); i++){
		a = u.prodA[i];
		b = u.prodB[i];

		for(int j = 0; j < n; j++){
			for(int q = 0; q < x[j].retSize(x[j]); q++){
				if(x[j].prod[q] == a) c++;
				if(x[j].prod[q] == b) d++;
			}

			if(c > 0 && d > 0){
				if(d > c) swap(c, d);
				u.wyst[i] += (int)(c/d);
			}
			c = 0; d = 0;
		}
	}
}

void paryProd::countProb(paryProd& u, pojProd z){
	float x;
	for(int i = 0; i < u.prodB.size(); i++){
		x = checkProb(u.wyst[i], z.wyst[u.prodA[i]]);
		if(x > 0){
			if(x > u.max) u.max = x;
		} 	
		u.paraProb.push_back(x);
	}
}

void paryProd::unProb(paryProd& u, float p){
	for(int i = 0; i < u.prodA.size(); i++)
		if(u.paraProb[i] <= p) u.paraProb[i] = -1;
}

bool paryProd::checkRep(paryProd u, int a, int b){
	for(int i = 0; i < u.prodA.size(); i++){
		if(u.prodA[i] == a){
			if(u.prodB[i] == b) return true;
		}
	}
	return false;
}

////////////Funkcje koszyka////////////
void koszyk::fill(koszyk& u, int pb, int pe, pojProd& z){
	int b = get_rand(pb, pe), tmp;
	z.wszystProd += b;
	
	for(int i = 0; i < b; i++){
		tmp = get_rand(0, z.nazwa.size() - 1);
		u.prod.push_back(tmp);
	}
}

void koszyk::out(koszyk u, pojProd& z, bool w, int q){	
	if(w) cout<<"\nKoszyk. "<< q+1 <<" {";
	for(int j = 0; j < u.prod.size(); j++){
		if(w)cout<<" "<<z.nazwa[u.prod[j]];
		z.wyst[u.prod[j]]++;
	}
	if(w) cout<<" }";
}

void koszyk::emptyProd(koszyk& u, pojProd x){
	for(int i = 0; i < u.prod.size(); i++){
		if(x.prodProb[u.prod[i]] < 0) u.prod.erase(u.prod.begin() + i);
	}
}

int koszyk::retSize(koszyk u){
	return u.prod.size();
}

////////////Funkcje produktów////////////
void pojProd::fill(pojProd& u, int p){
	string name;
	
	for(int i = 0; i < p; i++){
		system("CLS");
		
		cout<<"LISTA PRODUKTOW:";
		for(int j = 0; j < u.wyst.size(); j++){
			cout<<"\n"<<j+1<<".\t"<<u.nazwa[j];
		}
		
		cout<<"\n\nPodaj "<<i+1<<" produkt: ";
		cin>>name;
		
		u.nazwa.push_back(name);
		u.wyst.push_back(0);
	}
	
	system("CLS");	
}

void pojProd::probFill(pojProd& u){
	float x;

	for(int i = 0; i < u.wyst.size(); i++) {
		x = checkProb(u.wyst[i], u.wszystProd);
		if(x > 0){
			if(x > u.max) u.max = x;
		} 

		u.prodProb.push_back(x);
	}
}

void pojProd::unProb(pojProd& u, float uf){
	for(int i = 0; i < u.wyst.size(); i++)
		if(u.prodProb[i] <= uf) u.prodProb[i] = -1;
}
