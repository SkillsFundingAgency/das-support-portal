<#
	New-SelfSignedCertificate -Subject localhost -CertStoreLocation cert:\LocalMachine\My -DnsName localhost
#>

#Requires -RunAsAdministrator

$localhostpwd = ConvertTo-SecureString -String password -Force -AsPlainText
Import-PfxCertificate -FilePath .\Certificates\localhost.pfx -CertStoreLocation cert://LocalMachine/My -Password $localhostpwd -Exportable
Import-PfxCertificate -FilePath .\Certificates\localhost.pfx -CertStoreLocation cert://LocalMachine/Root -Password $localhostpwd -Exportable

