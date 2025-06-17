﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#pragma once

#ifndef GDHOSTWINDOW_H
#  define GDHOSTWINDOW_H

#  include "GdHostableUi.h"

class IGdHostWindow
{
public:
  virtual HWND windowHandle() = 0;
};

#endif
