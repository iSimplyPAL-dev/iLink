/* Queste Funzioni devono essere incluse nella pagina da dove viene richiamato lo stradario */
function OggettoEnte(
	Denominazione,
	Cap,
	Provincia,
	CodIstat,
	CodCNC,
	CodBelfiore,
	HaStradario
){
	/*alert(Denominazione);*/
	this.Denominazione = Denominazione;
	this.Cap = Cap;
	this.Provincia = Provincia;
	this.CodIstat = CodIstat;
	this.CodCNC = CodCNC;
	this.CodBelfiore = CodBelfiore;
	this.HaStradario = HaStradario;
}