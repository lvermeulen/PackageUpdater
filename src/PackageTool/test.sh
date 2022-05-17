#!/bin/bash
account_to_fork_from=InformatieVlaanderen
account_name=lvermeulen
folder_name=${PWD}
folder_base=$(basename $folder_name)

echo $account_name
echo $folder_name
echo $folder_base

#cd $folder_name
#
#echo Folder is: ${PWD}
#gh repo fork $account_to_fork_from/$folder_base --remote=false --clone=false
#git remote remove origin
#git remote remove upstream
#git remote add origin https://github.com/$account_name/$folder_base
#git remote add upstream https://github.com/$account_to_fork_from/$folder_base
#
#cd -
