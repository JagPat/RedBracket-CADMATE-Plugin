﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef _gduiBaseDialog_h
#define _gduiBaseDialog_h

#pragma pack(push, 8)

#if _MSC_VER >= 1000
#  pragma once
#endif

#ifndef kDialogOptionNone
#  define kDialogOptionNone 0
#endif

#ifndef kDialogOptionUseTips
#  define kDialogOptionUseTips 1
#endif

#ifndef kDialogOptionUseTipsForContextHelp
#  define kDialogOptionUseTipsForContextHelp 2
#endif

#ifndef _GSOFT_MAC_
#  pragma warning(push)
#  pragma warning(disable : 4275)
class GCAD_PORT CGdUiBaseDialog : public CDialog
{
  DECLARE_DYNAMIC(CGdUiBaseDialog);

public:
  CGdUiBaseDialog(UINT idd, CWnd* pParent = NULL, HINSTANCE hDialogResource = NULL);
  ~CGdUiBaseDialog();

protected:
  virtual GDUI_REPLY DoGdUiMessage(GDUI_NOTIFY notifyCode, UINT controlId, LPARAM lParam);
  virtual GDUI_REPLY OnNotifyControlChange(UINT controlId, LPARAM lParam);
  virtual GDUI_REPLY OnNotifyControlValid(UINT controlId, BOOL isValid);
  virtual GDUI_REPLY OnNotifyGeneric(UINT controlId, LPARAM lParam);
  virtual GDUI_REPLY OnNotifyUpdateTip(CWnd* control);

private:
  HINSTANCE m_commandResourceInstance;
  BOOL m_commandWindowWasEnabled;
  HWND m_commandWindowWithFocus;

protected:
  GDUI_COMMAND_STATE m_commandState;

  void BeginEditorCommand();
  void CancelEditorCommand();
  void CompleteEditorCommand(BOOL restoreDialogs = TRUE);
  void MakeDialogsVisible(BOOL visible);

public:
  BOOL EditorCommandCancelled();

protected:
  CString m_contextHelpFileName;
  CString m_contextHelpFullPathName;
  CString m_contextHelpPrefix;
  CString m_contextHelpMapFileName;

  virtual BOOL FindContextHelpFullPath(LPCTSTR fileName, CString& fullPath);

public:
  LPCTSTR GetContextHelpFileName();
  void SetContextHelpFileName(LPCTSTR pFileName);
  LPCTSTR GetContextHelpFullPathName();
  void SetContextHelpFullPathName(LPCTSTR pFullPathName);
  LPCTSTR GetContextHelpPrefix();
  void SetContextHelpPrefix(LPCTSTR pPrefix);
  LPCTSTR GetContextHelpMapFileName();
  void SetContextHelpMapFileName(LPCTSTR pFileName);

  CToolTipCtrl* GetToolTipCtrl();

private:
  BOOL m_bRunningModal;
  HINSTANCE m_hDialogResourceSaved;

  CFont m_substFont;

protected:
  int m_bUseTips;
  HINSTANCE m_hDialogResource;
  HICON m_hIcon;
  CGdUiTextTip* m_pTextTip;
  CToolTipCtrl* m_pToolTip;
  CString m_rootKey;

  friend class CGdUiAssist;
  static CGdUiAssist* m_pUIAssist;

protected:
  virtual void OnInitDialogBegin();
  virtual void OnInitDialogFinish();

public:
  virtual CWnd* AppMainWindow();
  virtual HINSTANCE AppResourceInstance();
  virtual LPCTSTR AppRootKey();
  void SetAppRootKey(LPCTSTR key);

  BOOL Create(LPCTSTR lpszTemplateName, CWnd* pParentWnd = NULL);
  BOOL Create(UINT nIDTemplate, CWnd* pParentWnd = NULL);

  virtual void EnableFloatingWindows(BOOL allow);
  virtual int IsMultiDocumentActivationEnabled();
  virtual int EnableMultiDocumentActivation(BOOL bEnable);
  virtual BOOL DoDialogHelp();

  int GetUseTips();
  void SetUseTips(int useTips);

  HICON GetDialogIcon();
  void SetDialogIcon(HICON hIcon);

public:
  BOOL DisplayData();
  virtual BOOL ExchangeData(BOOL bSaveAndValidate);
  BOOL ValidateData();

public:
  enum
  {
    IDD = 0
  };

public:
  BOOL PreTranslateMessage(MSG* pMsg) override;
  INT_PTR DoModal() override;

protected:
  void DoDataExchange(CDataExchange* pDX) override;
  void PostNcDestroy() override;
  BOOL PreCreateWindow(CREATESTRUCT& cs) override;

protected:
  BOOL OnInitDialog() override;
  afx_msg LRESULT OnGdUiMessage(WPARAM wParam, LPARAM lParam);
  afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
  afx_msg BOOL OnHelpInfo(HELPINFO* lpHelpInfo);
  afx_msg void OnActivate(UINT nState, CWnd* pWndOther, BOOL bMinimized);
  afx_msg void OnGdUiTimer(UINT_PTR nIDEvent);
  afx_msg BOOL OnNotify_ToolTipText(UINT id, NMHDR* pNMHDR, LRESULT* pResult);
  DECLARE_MESSAGE_MAP()
};
#  pragma warning(pop)

#endif

#pragma pack(pop)
#endif
