﻿function New-Config($class) {
	$sp = $(Get-Project).FullName;
	$idx = $sp.LastIndexOf('\');
	$prefixPath = $sp.Substring(0, $idx);
	$path = "$prefixPath\ConfigFiles"
	Start-Process "ConfigFileGenerator.ConfigCreator.exe" -arg "$path $class" -WindowStyle Hidden -Wait
	$pathConf = "$path\Data\$class.json"
	$prj = $(Get-Project)
	$res = $prj.ProjectItems.AddFromFile("$pathConf")
	$configItem = $prj.ProjectItems.Item("ConfigFiles").ProjectItems.Item('Data').ProjectItems.Item("$class.json")
	$configItem.Properties.Item("BuildAction").Value = 0
	$copyToOutput = $configItem.Properties.Item("CopyToOutputDirectory")
	$copyToOutput.Value = 2
}

Export-ModuleMember @( 'New-Config' )	