﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef DBDIMVAR_H
#define DBDIMVAR_H

virtual int dimadec() const;
virtual bool dimalt() const;
virtual int dimaltd() const;
virtual double dimaltf() const;
virtual double dimaltrnd() const;
virtual int dimalttd() const;
virtual int dimalttz() const;
virtual int dimaltu() const;
virtual int dimaltz() const;
virtual const GCHAR* dimapost() const;
virtual int dimarcsym() const;
virtual double dimasz() const;
virtual int dimatfit() const;
virtual int dimaunit() const;
virtual int dimazin() const;
virtual GcDbObjectId dimblk() const;
virtual GcDbObjectId dimblk1() const;
virtual GcDbObjectId dimblk2() const;
virtual double dimcen() const;
virtual GcCmColor dimclrd() const;
virtual GcCmColor dimclre() const;
virtual GcCmColor dimclrt() const;
virtual int dimdec() const;
virtual double dimdle() const;
virtual double dimdli() const;
virtual GCHAR dimdsep() const;
virtual double dimexe() const;
virtual double dimexo() const;
virtual int dimfrac() const;
virtual double dimgap() const;
virtual double dimjogang() const;
virtual int dimjust() const;
virtual GcDbObjectId dimldrblk() const;
virtual double dimlfac() const;
virtual bool dimlim() const;
virtual GcDbObjectId dimltex1() const;
virtual GcDbObjectId dimltex2() const;
virtual GcDbObjectId dimltype() const;
virtual int dimlunit() const;
virtual GcDb::LineWeight dimlwd() const;
virtual GcDb::LineWeight dimlwe() const;
virtual const GCHAR* dimpost() const;
virtual double dimrnd() const;
virtual bool dimsah() const;
virtual double dimscale() const;
virtual bool dimsd1() const;
virtual bool dimsd2() const;
virtual bool dimse1() const;
virtual bool dimse2() const;
virtual bool dimsoxd() const;
virtual int dimtad() const;
virtual int dimtdec() const;
virtual double dimtfac() const;
virtual int dimtfill() const;
virtual GcCmColor dimtfillclr() const;
virtual bool dimtih() const;
virtual bool dimtix() const;
virtual double dimtm() const;
virtual int dimtmove() const;
virtual bool dimtofl() const;
virtual bool dimtoh() const;
virtual bool dimtol() const;
virtual int dimtolj() const;
virtual double dimtp() const;
virtual double dimtsz() const;
virtual double dimtvp() const;
virtual GcDbObjectId dimtxsty() const;
virtual double dimtxt() const;
virtual int dimtzin() const;
virtual bool dimupt() const;
virtual int dimzin() const;
virtual bool dimfxlenOn() const;
virtual double dimfxlen() const;
virtual bool dimtxtdirection() const;
virtual double dimmzf() const;
virtual const GCHAR* dimmzs() const;
virtual double dimaltmzf() const;
virtual const GCHAR* dimaltmzs() const;

virtual Gcad::ErrorStatus setDimadec(int v);
virtual Gcad::ErrorStatus setDimalt(bool v);
virtual Gcad::ErrorStatus setDimaltd(int v);
virtual Gcad::ErrorStatus setDimaltf(double v);
virtual Gcad::ErrorStatus setDimaltmzf(double v);
virtual Gcad::ErrorStatus setDimaltmzs(const GCHAR* v);
virtual Gcad::ErrorStatus setDimaltrnd(double v);
virtual Gcad::ErrorStatus setDimalttd(int v);
virtual Gcad::ErrorStatus setDimalttz(int v);
virtual Gcad::ErrorStatus setDimaltu(int v);
virtual Gcad::ErrorStatus setDimaltz(int v);
virtual Gcad::ErrorStatus setDimapost(const GCHAR* v);
virtual Gcad::ErrorStatus setDimarcsym(int v);
virtual Gcad::ErrorStatus setDimasz(double v);
virtual Gcad::ErrorStatus setDimatfit(int v);
virtual Gcad::ErrorStatus setDimaunit(int v);
virtual Gcad::ErrorStatus setDimazin(int v);
virtual Gcad::ErrorStatus setDimblk(GcDbObjectId v);
virtual Gcad::ErrorStatus setDimblk1(GcDbObjectId v);
virtual Gcad::ErrorStatus setDimblk2(GcDbObjectId v);
virtual Gcad::ErrorStatus setDimcen(double v);
virtual Gcad::ErrorStatus setDimclrd(const GcCmColor& v);
virtual Gcad::ErrorStatus setDimclre(const GcCmColor& v);
virtual Gcad::ErrorStatus setDimclrt(const GcCmColor& v);
virtual Gcad::ErrorStatus setDimdec(int v);
virtual Gcad::ErrorStatus setDimdle(double v);
virtual Gcad::ErrorStatus setDimdli(double v);
virtual Gcad::ErrorStatus setDimdsep(GCHAR v);
virtual Gcad::ErrorStatus setDimexe(double v);
virtual Gcad::ErrorStatus setDimexo(double v);
virtual Gcad::ErrorStatus setDimfrac(int v);
virtual Gcad::ErrorStatus setDimgap(double v);
virtual Gcad::ErrorStatus setDimjogang(double v);
virtual Gcad::ErrorStatus setDimjust(int v);
virtual Gcad::ErrorStatus setDimldrblk(GcDbObjectId v);
virtual Gcad::ErrorStatus setDimlfac(double v);
virtual Gcad::ErrorStatus setDimlim(bool v);
virtual Gcad::ErrorStatus setDimltex1(GcDbObjectId v);
virtual Gcad::ErrorStatus setDimltex2(GcDbObjectId v);
virtual Gcad::ErrorStatus setDimltype(GcDbObjectId v);
virtual Gcad::ErrorStatus setDimlunit(int v);
virtual Gcad::ErrorStatus setDimlwd(GcDb::LineWeight v);
virtual Gcad::ErrorStatus setDimlwe(GcDb::LineWeight v);
virtual Gcad::ErrorStatus setDimmzf(double v);
virtual Gcad::ErrorStatus setDimmzs(const GCHAR* v);
virtual Gcad::ErrorStatus setDimpost(const GCHAR* v);
virtual Gcad::ErrorStatus setDimrnd(double v);
virtual Gcad::ErrorStatus setDimsah(bool v);
virtual Gcad::ErrorStatus setDimscale(double v);
virtual Gcad::ErrorStatus setDimsd1(bool v);
virtual Gcad::ErrorStatus setDimsd2(bool v);
virtual Gcad::ErrorStatus setDimse1(bool v);
virtual Gcad::ErrorStatus setDimse2(bool v);
virtual Gcad::ErrorStatus setDimsoxd(bool v);
virtual Gcad::ErrorStatus setDimtad(int v);
virtual Gcad::ErrorStatus setDimtdec(int v);
virtual Gcad::ErrorStatus setDimtfac(double v);
virtual Gcad::ErrorStatus setDimtfill(int v);
virtual Gcad::ErrorStatus setDimtfillclr(const GcCmColor& v);
virtual Gcad::ErrorStatus setDimtih(bool v);
virtual Gcad::ErrorStatus setDimtix(bool v);
virtual Gcad::ErrorStatus setDimtm(double v);
virtual Gcad::ErrorStatus setDimtmove(int v);
virtual Gcad::ErrorStatus setDimtofl(bool v);
virtual Gcad::ErrorStatus setDimtoh(bool v);
virtual Gcad::ErrorStatus setDimtol(bool v);
virtual Gcad::ErrorStatus setDimtolj(int v);
virtual Gcad::ErrorStatus setDimtp(double v);
virtual Gcad::ErrorStatus setDimtsz(double v);
virtual Gcad::ErrorStatus setDimtvp(double v);
virtual Gcad::ErrorStatus setDimtxsty(GcDbObjectId v);
virtual Gcad::ErrorStatus setDimtxt(double v);
virtual Gcad::ErrorStatus setDimtxtdirection(bool v);
virtual Gcad::ErrorStatus setDimtzin(int v);
virtual Gcad::ErrorStatus setDimupt(bool v);
virtual Gcad::ErrorStatus setDimzin(int v);
virtual Gcad::ErrorStatus setDimblk(const GCHAR* v);
virtual Gcad::ErrorStatus setDimblk1(const GCHAR* v);
virtual Gcad::ErrorStatus setDimblk2(const GCHAR* v);
virtual Gcad::ErrorStatus setDimldrblk(const GCHAR* v);
virtual Gcad::ErrorStatus setDimfxlenOn(bool v);
virtual Gcad::ErrorStatus setDimfxlen(double v);

#endif
