// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the Chromium Projects, licensed under the BSD-style license. More info in THIRD-PARTY-NOTICES file.
// Copyright (c) 2013 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

namespace Zu.Chrome.DriverCore
{
    class is_option_element_toggleable
    {
        public const string JsSource = @"
function() {

function isOptionElementToggleable(option) {
  if (option.tagName.toLowerCase() != 'option')
    throw new Error('element is not an option');
  for (var parent = option.parentElement;
       parent;
       parent = parent.parentElement) {
    if (parent.tagName.toLowerCase() == 'select') {
      return parent.multiple;
    }
  }
  throw new Error('option element is not in a select');
}
;
return isOptionElementToggleable.apply(null, arguments) 
}
";
    }
}
