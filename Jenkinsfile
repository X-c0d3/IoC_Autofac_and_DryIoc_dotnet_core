pipeline {
  agent {
    node {
        label 'Raspberry Pi 2'
    }
  }
  stages {
    stage('Checkout code') {
      steps {
        checkout scm
      }
    }

    stage('Clean') {
     steps {
       sh 'dotnet clean'
     }
    }

    stage('Restore') {
      steps {
        sh 'dotnet restore'
      }
    }

    stage('Build') {
      steps {
        sh 'dotnet build -c Release'
	    }
    }

    stage('Test') {
      steps {
        sh 'dotnet test ./IoC.Test'
	    }
    }
 
    stage('Deploy') {
      steps {
        sh 'dotnet publish ./IoC.Web/IoC.Web.csproj -c Release -o Deployed -r linux-arm64'
      }
    }

    // ################## DOCKER #########################
    // stage('Build Docker'){
    //  steps {
    //      sh "docker build -t coreb0tz/jenkins-demo:${BUILD_NUMBER} --no-cache ."
    //  }
    // }

    // stage('Docker tag'){
    //  steps {
    //      sh "docker tag coreb0tz/jenkins-demo:${BUILD_NUMBER} coreb0tz/jenkins-demo:latest"
    //  }
    // }

    // stage('Deploy docker'){
    //   if(env.BRANCH_NAME == 'master'){    
    //     sh "docker run -d -p 80:80 coreb0tz/jenkins-demo:latest"
    //     //sh 'docker push localhost:5000/react-app'
    //     // sh 'docker rmi -f react-app localhost:5000/react-app'
    //   }
    // }
    // ###################################################
    
    stage('Compress to artifacts file ') {
      steps {
        sh "tar -zcf ./${JOB_NAME}_1.0.1.${env.BUILD_NUMBER}.tar.gz ./IoC.Web/Deployed/"
      }
    }
    
    stage('Send Email Notify') {
      steps {
        emailext body: 'Build execution completed!', subject: 'Build execution completed !', to: 'rockdevper@gmail.com'
      }
    }
  }
  post {
        success {
            // archiveArtifacts "./*.tar.gz"
            echo 'Build concluido com sucesso.'
            sh "curl -X POST -H 'Authorization: Bearer ${env.Line_AuthorizationBearer}' -F 'message=Build version : 1.0.1.${env.BUILD_NUMBER}  - (${NODE_NAME}) - Success' ${env.Line_Notify_URL}"
        }
        failure {
            echo 'Erro ao processar o build.'
            sh "curl -X POST -H 'Authorization: Bearer ${env.Line_AuthorizationBearer}' -F 'message=Build version : 1.0.1.${env.BUILD_NUMBER}  - (${NODE_NAME}) - Fail' ${env.Line_Notify_URL}"
        }
  }
}