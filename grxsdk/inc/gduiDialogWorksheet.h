﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef GDUIDLGWORKSHEET_H
#define GDUIDLGWORKSHEET_H

class CGdUiWorksheetDraw;

class GCAD_PORT CGdUiDialogWorksheet : public CGdUiDialog
{
  friend class CGdUiWorksheetThemeModReactor;
  DECLARE_DYNAMIC(CGdUiDialogWorksheet);

public:
  ~CGdUiDialogWorksheet();

  CGdUiTheme* GetTheme() const;
  CGdUiTheme* SetTheme(CGdUiTheme* pTheme);

  static CGdUiWorksheetDraw* SetDefaultDraw(CGdUiWorksheetDraw* pDraw);
  void SetDraw(const CGdUiWorksheetDraw* pDraw);

  virtual void ThemeModified(GdUiThemeElement element);

public:
  BOOL CreateEx(DWORD dwExStyle,
                LPCTSTR lpszClassName,
                LPCTSTR lpszWindowName,
                DWORD dwStyle,
                int x,
                int y,
                int nWidth,
                int nHeight,
                HWND hWndParent,
                HMENU nIDorHMenu,
                LPVOID lpParam = NULL) override;

  BOOL CreateEx(DWORD dwExStyle,
                LPCTSTR lpszClassName,
                LPCTSTR lpszWindowName,
                DWORD dwStyle,
                const RECT& rect,
                CWnd* pParentWnd,
                UINT nID,
                LPVOID lpParam = NULL) override;

  BOOL DestroyWindow() override;
  BOOL PreCreateWindow(CREATESTRUCT& cs) override;
  BOOL PreTranslateMessage(MSG* pMsg) override;
  BOOL OnInitDialog() override;

protected:
  void PostNcDestroy() override;
  void DoDataExchange(CDataExchange* pDX) override;

protected:
  HICON m_hIcon;

  CGdUiDialogWorksheet(UINT ID, CWnd* pParent = NULL, HINSTANCE hResInst = NULL);
  CGdUiDialogWorksheet(UINT ID, const CGdUiWorksheetDraw& draw, CWnd* pParent = NULL, HINSTANCE hResInst = NULL);

  afx_msg void OnDestroy();
  afx_msg void OnClose();
  afx_msg void OnPaint();
  afx_msg HCURSOR OnQueryDragIcon();
  afx_msg void OnSize(UINT nType, int cx, int cy);
  afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
  afx_msg BOOL OnEraseBkgnd(CDC* pDC);
  afx_msg void OnIconEraseBkgnd(CDC* pDC);
  afx_msg void OnSetFocus(CWnd* pOldWnd);
  afx_msg void OnKillFocus(CWnd* pNewWnd);
  afx_msg void OnStyleChanged(int nStyleType, LPSTYLESTRUCT lpStyleStruct);
  afx_msg void OnStyleChanging(int nStyleType, LPSTYLESTRUCT lpStyleStruct);
  afx_msg void OnSizing(UINT nSide, LPRECT lpRect);
  afx_msg void OnMoving(UINT nSide, LPRECT lpRect);
  afx_msg void OnMouseMove(UINT nHitTest, CPoint point);
  afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
  afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
  afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
  afx_msg void OnMButtonDblClk(UINT nFlags, CPoint point);
  afx_msg void OnMButtonDown(UINT nFlags, CPoint point);
  afx_msg void OnMButtonUp(UINT nFlags, CPoint point);
  afx_msg void OnRButtonDblClk(UINT nFlags, CPoint point);
  afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
  afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
  afx_msg void OnMeasureItem(int nIDCtl, LPMEASUREITEMSTRUCT lpMeasureItemStruct);
  afx_msg void OnDrawItem(int nIDCtl, LPDRAWITEMSTRUCT lpDrawItemStruct);
  afx_msg void OnGetMinMaxInfo(MINMAXINFO* lpMMI);
  afx_msg BOOL OnNcActivate(BOOL bActive);
  afx_msg void OnNcCalcSize(BOOL bCalcValidRects, NCCALCSIZE_PARAMS* lpncsp);
  afx_msg BOOL OnNcCreate(LPCREATESTRUCT lpCreateStruct);
  afx_msg void OnNcDestroy();
#if _MSC_VER < 1400
  afx_msg UINT OnNcHitTest(CPoint point);
#else
  afx_msg LRESULT OnNcHitTest(CPoint point);
#endif
  afx_msg void OnNcLButtonDblClk(UINT nHitTest, CPoint point);
  afx_msg void OnNcLButtonDown(UINT nHitTest, CPoint point);
  afx_msg void OnNcLButtonUp(UINT nHitTest, CPoint point);
  afx_msg void OnNcMButtonDblClk(UINT nHitTest, CPoint point);
  afx_msg void OnNcMButtonDown(UINT nHitTest, CPoint point);
  afx_msg void OnNcMButtonUp(UINT nHitTest, CPoint point);
  afx_msg void OnNcMouseMove(UINT nHitTest, CPoint point);
  afx_msg void OnNcPaint();
  afx_msg void OnNcRButtonDblClk(UINT nHitTest, CPoint point);
  afx_msg void OnNcRButtonDown(UINT nHitTest, CPoint point);
  afx_msg void OnNcRButtonUp(UINT nHitTest, CPoint point);
  DECLARE_MESSAGE_MAP()

private:
  static CGdUiWorksheetDraw* s_pDefaultDraw;
  static CGdUiWorksheetDraw* s_PlatformDefaultDraw;
  CGdUiWorksheetDraw* m_pDraw;
};

#endif