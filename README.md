# Azure Computer Vision Trainer
This example will take a blob storage connection (and container) and train a custom computer vision model.

Below are the parameters it takes

```c#

        string strTag                       = args[0];
        string strConnectionString          = args[1];
        string strContainer                 = args[2];
        string strProject                   = args[3];
        string strKey                       = args[4];
        string strEndPt                     = args[5];

```


<b>Tag</b> is what you want to tag the images as, like all these images are "cat" or "dog"

<b>Connection String</b> string is to your Azure storage account

<b>Container</b> is the storage container with your trainning images

<b>Project</b> is the name of the computer vision project

<b>Key</b> is the key to your computer vision training model

<b>EndPt</b> is the end point to your computer vision training model
